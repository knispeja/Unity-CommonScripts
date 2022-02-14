namespace KnispelCommon.DesignPattern
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "New Event", menuName = "KnispelScriptables/ListenableEvent")]
	public class ListenableEvent : ScriptableObject
	{
		private ISet<EventListener> listeners = new HashSet<EventListener>();

		/// <summary>
		/// Registers a listener to listen to this event. It will be notified when the event fires.
		/// If that listener was already registered, this will do nothing.
		/// </summary>
		/// <param name="listener">The listener to register.</param>
		/// <returns>True if and only if the listener was successfully added.</returns>
		public bool AddListener(EventListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException(nameof(listener));
			}

			return listeners.Add(listener);
		}

		/// <summary>
		/// Deregisters a listener that is listening to this event.
		/// If that listener was not registered, this will do nothing.
		/// </summary>
		/// <param name="listener">The listener to deregister.</param>
		/// <returns>True if and only if the listener was successfully removed.</returns>
		public bool RemoveListener(EventListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException(nameof(listener));
			}

			return listeners.Remove(listener);
		}

		/// <summary>
		/// Deregisters all listeners that are listening to this event.
		/// </summary>
		public void RemoveAllListeners()
		{
			listeners.Clear();
		}

		/// <summary>
		/// Raises the listenable event, notifying all <see cref="EventListener"/> objects listening.
		/// Any exceptions thrown by listeners are rethrown as an <see cref="AggregateException"/>.
		/// </summary>
		public void RaiseEvent()
		{
			ICollection<Exception> exceptions = null;
			foreach (EventListener listener in listeners)
			{
				if (listener != null)
				{
					try
					{
						listener.OnEventRaised();
					}
					catch (Exception e)
					{
						if (exceptions == null)
						{
							exceptions = new List<Exception>() { e };
						}
						else
						{
							exceptions.Add(e);
						}
					}
				}
			}

			if (exceptions != null)
			{
				throw new AggregateException(exceptions);
			}
		}
	}
}
