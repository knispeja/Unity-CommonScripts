using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsWrapper
{
#if UNITY_EDITOR
	private const bool _ENABLE_ANALYTICS_IN_EDITOR = false;
	private const bool _LOG_ANALYTICS_EVENTS_IN_EDITOR = true;

	static AnalyticsWrapper()
	{
		// Diable analytics in editor only to avoid pollution
		// Remember than you CAN reset Unity Analytics stats completely
		Analytics.enabled = _ENABLE_ANALYTICS_IN_EDITOR;
	}
#endif

	public static void SendEventSafe(string eventName, IDictionary<string, object> eventData = null)
	{
		SendEventSafe(eventName, () => eventData);
	}

	public static void SendEventSafe(string eventName, Func<IDictionary<string, object>> makeEventDataFunc)
	{
		AnalyticsResult? result = null;
		Exception exception = null;

		try
		{
			IDictionary<string, object> eventData = null;

			try
			{
				eventData = makeEventDataFunc();
			}
			catch (Exception e)
			{
				Debug.LogWarning($"Failed to create event data for analytics event {eventName}: {e.Message}");
				eventData = new Dictionary<string, object>() {{ "error", e.Message }};
			}

#if UNITY_EDITOR
			if (_LOG_ANALYTICS_EVENTS_IN_EDITOR && eventData != null)
			{
				var builder = new StringBuilder();

				builder.AppendLine($"--- Analytics Event : {eventName} ---");

				foreach (KeyValuePair<string, object> kvp in eventData)
				{
					builder.AppendLine($"{kvp.Key} : {kvp.Value}");
				}
				Debug.Log(builder);
			}
#endif

			result = Analytics.CustomEvent(eventName, eventData);

			if (result == AnalyticsResult.Ok || result == AnalyticsResult.AnalyticsDisabled)
			{
				return;
			}
		}
		catch (Exception e)
		{
			exception = e;
		}

		if (exception != null || result.HasValue)
		{
			string issueReason;
			if (exception != null)
			{
				issueReason = $"An exception occurred with message '{exception.Message}'";
			}
			else
			{
				issueReason = Enum.GetName(typeof(AnalyticsResult), result.Value);
			}

			Debug.LogWarning($"Unity Analytics encountered an issue, reason: {issueReason}");
		}
	}
}
