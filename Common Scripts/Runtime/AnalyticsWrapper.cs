using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Safely wraps the Unity Analytics library with additional logging and
/// preprocessor configuration options for testing with the Editor.
/// </summary>
public static class AnalyticsWrapper
{
#if UNITY_EDITOR || DISABLE_ANALYTICS
	static AnalyticsWrapper()
	{
#if DISABLE_ANALYTICS
		Debug.LogWarning("Unity Analytics is completely disabled (not just in the editor).");
#endif

		// Diable analytics in editor by default to avoid pollution
		// Remember than you CAN reset Unity Analytics stats completely
#if ENABLE_ANALYTICS_IN_EDITOR && !DISABLE_ANALYTICS
		Analytics.enabled = true;
#else
		Analytics.enabled = false;
#endif
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

#if UNITY_EDITOR && LOG_ANALYTICS_EVENTS_IN_EDITOR
			if (eventData != null)
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
