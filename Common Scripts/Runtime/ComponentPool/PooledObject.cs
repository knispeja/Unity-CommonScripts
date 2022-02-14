namespace KnispelCommon.ComponentPool
{
	using System;
	using UnityEngine;

	internal class PooledObject : MonoBehaviour
	{
		private Action enableAction;
		private Action disableAction;
		private Action destroyAction;

		public void Initialize(Action actionOnEnable, Action actionOnDisable, Action actionOnDestroy)
		{
			enableAction = actionOnEnable;
			disableAction = actionOnDisable;
			destroyAction = actionOnDestroy;
		}

		private void OnEnable()
		{
			enableAction.Invoke();
		}

		private void OnDisable()
		{
			disableAction.Invoke();
		}

		private void OnDestroy()
		{
			destroyAction.Invoke();
		}
	}
}
