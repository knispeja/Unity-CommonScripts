namespace KnispelCommon.ComponentPool
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class ComponentPool<T> where T : Component
	{
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

		public ComponentPool(int poolSize, GameObject prefab, Transform parent = null) : this(CreatePoolFromPrefab(poolSize, prefab, parent))
		{
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
						Debug.LogWarning("Component pool must be provided inactive objects to begin with");
					}
				}

				if (!warnDuplicates && !quickComponents.Add(component))
				{
					Debug.LogWarning("Component pool should not be provided duplicate components");
				}

				if (!warnParent && component.transform.parent != ComponentParent)
				{
					Debug.LogWarning("Component pool should be provided objects that share a common parent");
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

		private static Queue<T> CreatePoolFromPrefab(int poolSize, GameObject prefab, Transform parent)
		{
			var result = new Queue<T>(poolSize);

			if (parent == null)
			{
				// Avoid pooling these at the root of the GameObject tree, automatically create a container
				parent = ProvidePoolContainer(prefab.name);
			}

			for (int i = 0; i < poolSize; i++)
			{
				GameObject spawned = UnityEngine.Object.Instantiate(prefab, parent);
				T component = spawned.GetComponent<T>();

				if (component == null)
				{
					throw new ArgumentException($"Provided prefab does not contain required component {typeof(T).Name}");
				}

				spawned.SetActive(false);
				result.Enqueue(component);
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

		public T InstantiateFromPool()
		{
			if (inactivePooledComponents.Count <= 0)
			{
				Debug.LogWarning($"Pool size of exceeded -- failed to create object. Expanding the pool of objects is recommended.");
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
				Debug.LogWarning("Pooled component was instantiated in an inactive tree -- this is not supported");
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
				Debug.LogWarning($"{nameof(ComponentPool<T>)} does not support pooled objects being activated without using {nameof(InstantiateFromPool)}.");
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
			}
		}
	}
}
