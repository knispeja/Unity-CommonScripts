namespace KnispelCommon.ComponentPool
{
	using KnispelCommon.Logging;
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class ComponentPool<T> where T : Component
	{
		private T originalPrefab = null;

		private HashSet<T> activePooledComponents;
		private Queue<T> inactivePooledComponents;

		/// <summary>
		/// Do not deactivate components while iterating this enumerable.
		/// Instead, either make a copy or deactivate the <see cref="ComponentParent"/> to to deactivate all components.
		/// </summary>
		public IEnumerable<T> ActiveComponents => activePooledComponents;

		public int ActiveComponentCount => activePooledComponents.Count;

		public int TotalComponentCount { get; private set; }

		public Transform ComponentParent { get; private set; }

		public LogLevel LogLevelOnComponentDestroy { get; set; } = LogLevel.NONE;

		public ComponentPool(int poolSize, T prefab, Transform parent = null) : this(CreatePoolFromPrefab(poolSize, prefab, parent))
		{
			originalPrefab = prefab;
		}

		public ComponentPool(int poolSize, T prefab, string customName) : this(CreatePoolFromPrefab(poolSize, prefab, customName))
		{
			originalPrefab = prefab;
		}

		public ComponentPool(Queue<T> inactiveComponentPool)
		{
			const int MIN_POOL_SIZE = 1;
			if (inactiveComponentPool.Count < MIN_POOL_SIZE)
			{
				throw new ArgumentException($"Pool size must be at least {MIN_POOL_SIZE}");
			}

			ComponentParent = inactiveComponentPool.Peek().transform.parent;

#if UNITY_EDITOR
			// Editor-only pool validation
			bool warnActive = false;
			bool warnDuplicates = false;
			bool warnParent = false;
			var quickComponents = new HashSet<T>();
			foreach (T component in inactiveComponentPool)
			{
				if (component.gameObject.activeSelf)
				{
					component.gameObject.SetActive(false);
					if (!warnActive)
					{
						warnActive = true;
						LogWrapper.LogWarning("Component pool must be provided inactive objects to begin with, but was provided at least one active object. This message will only show up in the editor.");
					}
				}

				if (!warnDuplicates && !quickComponents.Add(component))
				{
					LogWrapper.LogWarning("Component pool should not be provided duplicate components. This message will only show up in the editor.");
				}

				if (!warnParent && component.transform.parent != ComponentParent)
				{
					LogWrapper.LogWarning("Component pool should be provided objects that share a common parent. This message will only show up in the editor.");
				}
			}
#endif

			TotalComponentCount = inactiveComponentPool.Count;

			activePooledComponents = new HashSet<T>();
			inactivePooledComponents = inactiveComponentPool;
			foreach (T component in inactivePooledComponents)
			{
				// Attach custom components to the pooled objects that help manage the pool
				component.gameObject
					.AddComponent<PooledObject>()
					.Initialize(
						() => OnPooledObjectActivate(component),
						() => OnPooledObjectDeactivate(component),
						() => OnPooledObjectDestroy(component)
					);
			}
		}

		private static Queue<T> CreatePoolFromPrefab(int poolSize, T prefab, string customName)
		{
			Transform parent = ProvidePoolContainer(customName);
			return CreatePoolFromPrefab(poolSize, prefab, parent);
		}

		private static Queue<T> CreatePoolFromPrefab(int poolSize, T prefab, Transform parent)
		{
			var result = new Queue<T>(poolSize);

			if (parent == null)
			{
				// Avoid pooling these at the root of the GameObject tree, automatically create a container
				parent = ProvidePoolContainer(prefab.name);
			}

			for (int i = 0; i < poolSize; i++)
			{
				T spawned = UnityEngine.Object.Instantiate(prefab, parent);
				spawned.gameObject.SetActive(false);
				result.Enqueue(spawned);
			}

			return result;
		}

		public static Transform ProvidePoolContainer(string name)
		{
			return new GameObject($"{nameof(ComponentPool<Component>)}-{name}").transform;
		}

		public bool IsEmpty()
		{
			return inactivePooledComponents.Count <= 0;
		}

		public T InstantiateFromPool(LogLevel logLevelWhenPoolEmpty = LogLevel.ERROR)
		{
			return InstantiateFromPoolInternal(logLevelWhenPoolEmpty);
		}

		public T InstantiateFromPoolOrOriginalPrefab(LogLevel logLevelWhenPoolEmpty = LogLevel.ERROR)
		{
			if (originalPrefab == null)
			{
				throw new InvalidOperationException("No prefab was ever used to set up this component pool");
			}

			return InstantiateFromPoolOrPrefab(originalPrefab, logLevelWhenPoolEmpty);
		}

		public T InstantiateFromPoolOrPrefab(T prefab, LogLevel logLevelWhenPoolEmpty = LogLevel.ERROR)
		{
			T result = InstantiateFromPoolInternal(logLevelWhenPoolEmpty);
			if (result == null)
			{
				result = UnityEngine.Object.Instantiate(prefab, ComponentParent);
				result.gameObject.SetActive(false);

				TotalComponentCount++;
				inactivePooledComponents.Enqueue(result);
			}

			return result;
		}

		private T InstantiateFromPoolInternal(LogLevel logLevelWhenPoolEmpty)
		{
			if (inactivePooledComponents.Count <= 0)
			{
				LogWrapper.LogFormat(logLevelWhenPoolEmpty, "Pool size of {0} exceeded -- failed to create object. Expanding the pool of objects is recommended.", TotalComponentCount);
				return null;
			}

			if (!ComponentParent.gameObject.activeSelf)
			{
				ComponentParent.gameObject.SetActive(true);
			}

			T component = inactivePooledComponents.Dequeue();
			component.gameObject.SetActive(true);

#if UNITY_EDITOR
			if (!component.gameObject.activeInHierarchy)
			{
				LogWrapper.LogWarningWithContext("Pooled component was instantiated in an inactive tree -- this is not supported. This message will only show up in the editor.", component.gameObject);
			}
#endif

			return component;
		}

		public void ReturnAllToPool()
		{
			ComponentParent.gameObject.SetActive(false);
		}

		public void ReturnToPool(T component)
		{
			OnPooledObjectDeactivate(component);
		}

		internal void OnPooledObjectActivate(T component)
		{
			if (!activePooledComponents.Add(component))
			{
				LogWrapper.LogWarningWithContext($"{nameof(ComponentPool<T>)} does not support pooled objects being activated without using {nameof(InstantiateFromPool)}.", component.gameObject);
			}
		}

		internal void OnPooledObjectDeactivate(T component)
		{
			if (component.gameObject.activeSelf)
			{
				// Component was disabled by hierarchy, force disable so it doesn't come back
				component.gameObject.SetActive(false);
			}

			if (activePooledComponents.Remove(component))
			{
				inactivePooledComponents.Enqueue(component);
			}
		}

		internal void OnPooledObjectDestroy(T component)
		{
			if (component.gameObject.scene.isLoaded)
			{
				// Scene is not shutting down, so just deal with this situation normally
				activePooledComponents.Remove(component);
				TotalComponentCount--;

				LogWrapper.LogFormat(LogLevelOnComponentDestroy, "Component under parent {0} was destroyed -- deactivating is recommended for use with component pool", ComponentParent.name);
			}
		}
	}
}
