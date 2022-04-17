namespace KnispelCommon.DesignPattern
{
	using KnispelCommon.Logging;
	using System;
	using UnityEngine;
	using UnityEngine.Events;

	public class EventListener : MonoBehaviour
	{
		public UnityEvent ActionOnEventRaise { get; set; } = null;

		[SerializeField]
		private ListenableEvent eventToListenTo = null;

		[Header("Configuration")]
		[SerializeField]
		private bool listenWhileDisabled = false;

		[SerializeField]
		private bool throwOnEventIfActionNotSet = true;

		public void OnEventRaised()
		{
			if (ActionOnEventRaise == null)
			{
				if (throwOnEventIfActionNotSet)
				{
					throw new InvalidOperationException($"Event listener {gameObject.name} has no action to perform");
				}
			}
			else
			{
				ActionOnEventRaise.Invoke();
			}
		}

		private void Awake()
		{
			if (eventToListenTo == null)
			{
				LogWrapper.LogWarningFormat("Event listener {0} has no event to listen to", gameObject.name);
			}
		}

		private void OnEnable()
		{
			if (eventToListenTo != null)
			{
				eventToListenTo.AddListener(this);
			}
		}

		private void OnDisable()
		{
			if (!listenWhileDisabled && eventToListenTo != null)
			{
				eventToListenTo.RemoveListener(this);
			}
		}

		private void OnDestroy()
		{
			// Only need to remove here if OnDisable() won't remove
			if (listenWhileDisabled && eventToListenTo != null)
			{
				eventToListenTo.RemoveListener(this);
			}
		}
	}
}
