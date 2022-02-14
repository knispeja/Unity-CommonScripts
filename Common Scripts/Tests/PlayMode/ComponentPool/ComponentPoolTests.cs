namespace KnispelCommon.Tests.ComponentPool
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using KnispelCommon.ComponentPool;
	using NUnit.Framework;
	using UnityEngine;
	using UnityEngine.TestTools;

	public class ComponentPoolTests
	{
		[UnityTest]
		public IEnumerator Constructor_EmptyQueue_Throws()
		{
			Assert.Throws<ArgumentException>(() => new ComponentPool<Component>(new Queue<Component>(0)));
			yield return null;
		}

		[UnityTest]
		public IEnumerator ComponentPool_RigorousPoolManipulation(
				[Values(1, 5)] int poolSize,
				[Values(true, false)] bool nullComponentParentParam
			)
		{
			const string prefabMessage = "Prefab_Tag";
			var prefab = new GameObject("Prefab_Object");
			var component = prefab.AddComponent<TestComponent>();
			component.message = prefabMessage;

			Transform componentParent = nullComponentParentParam ? null : new GameObject("Component_Parent").transform;

			var instance = new ComponentPool<TestComponent>(poolSize, prefab, componentParent);
			Assert.AreEqual(poolSize, instance.TotalComponentCount);

			if (nullComponentParentParam)
			{
				Assert.IsNotNull(instance.ComponentParent);
			}
			else
			{
				Assert.AreEqual(componentParent, instance.ComponentParent);
			}

			// Loop, emptying the pool and making sure all components are unique and have the correct parent
			var expectedActiveComponents = new HashSet<TestComponent>();
			while (expectedActiveComponents.Count < poolSize)
			{
				AssertActiveComponents(expectedActiveComponents, poolSize, instance);

				TestComponent retrievedComponent = RetrieveFromPoolAndValidate(expectedActiveComponents, instance);

				// Do some basic validation on the components being returned
				Assert.AreEqual(prefabMessage, retrievedComponent.message);
				Assert.AreEqual(instance.ComponentParent, retrievedComponent.transform.parent);
			}

			AssertPoolEmpty(instance);

			// Completely re-fill pool by disabling all objects
			foreach (TestComponent activeComponent in new HashSet<TestComponent>(expectedActiveComponents)) // Copy to avoid "collection modified exception"
			{
				instance.ReturnToPool(activeComponent);
				expectedActiveComponents.Remove(activeComponent);

				AssertActiveComponents(expectedActiveComponents, poolSize, instance);
			}

			// Empty pool
			while (expectedActiveComponents.Count < poolSize)
			{
				RetrieveFromPoolAndValidate(expectedActiveComponents, instance);
				AssertActiveComponents(expectedActiveComponents, poolSize, instance);
			}

			AssertPoolEmpty(instance);

			instance.ReturnAllToPool();
			expectedActiveComponents.Clear();
			AssertActiveComponents(expectedActiveComponents, poolSize, instance);

			// Retrieve one from pool and try destroying it
			TestComponent toDestroy = RetrieveFromPoolAndValidate(expectedActiveComponents, instance);
			AssertActiveComponents(expectedActiveComponents, poolSize, instance);
			expectedActiveComponents.Remove(toDestroy);
			UnityEngine.Object.Destroy(toDestroy.gameObject);
			AssertActiveComponents(expectedActiveComponents, poolSize, instance);

			yield return null;
		}

		private TestComponent RetrieveFromPoolAndValidate(HashSet<TestComponent> expectedActiveComponents, ComponentPool<TestComponent> componentPool)
		{
			TestComponent retrievedComponent = componentPool.InstantiateFromPool();
			Assert.IsNotNull(retrievedComponent, "Pool should have open space");
			Assert.IsTrue(expectedActiveComponents.Add(retrievedComponent), "Components returned by the component pool should be unique");
			return retrievedComponent;
		}

		private void AssertActiveComponents<T>(ICollection<T> expectedActiveComponents, int poolSize, ComponentPool<T> componentPool) where T : Component
		{
			foreach (T component in expectedActiveComponents)
			{
				Assert.That(component.gameObject.activeInHierarchy);
			}

			Assert.AreEqual(poolSize - expectedActiveComponents.Count <= 0 ? true : false, componentPool.IsEmpty());
			Assert.AreEqual(expectedActiveComponents.Count, componentPool.ActiveComponents.Count());
			Assert.AreEqual(expectedActiveComponents.Count, componentPool.ActiveComponentCount);
			CollectionAssert.AreEquivalent(expectedActiveComponents, componentPool.ActiveComponents);
		}

		private void AssertPoolEmpty<T>(ComponentPool<T> componentPool) where T : Component
		{
			Assert.IsTrue(componentPool.IsEmpty());
			Assert.IsNull(componentPool.InstantiateFromPool(), "Pool size should be exceeded and return null");
			Assert.IsNull(componentPool.InstantiateFromPool(), "Pool size should still be exceeded");
		}
	}
}
