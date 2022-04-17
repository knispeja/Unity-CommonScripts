namespace KnispelCommon.Logging
{
	using System;
	using System.Diagnostics;

	public static class LogWrapper
	{
		#region ERROR
		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogError(object message)
		{
			UnityEngine.Debug.LogError(message);
		}

		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogErrorWithContext(object message, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogError(message, context);
		}

		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogErrorFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(message, args);
		}

		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogErrorFormatWithContext(UnityEngine.Object context, string message, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(context, message, args);
		}

		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogError(Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}

		[Conditional("LOGLEVEL_ERROR")]
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogErrorWithContext(Exception exception, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogException(exception, context);
		}
		#endregion

		#region WARNING
		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogWarning(object message)
		{
			UnityEngine.Debug.LogWarning(message);
		}

		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogWarningWithContext(object message, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}

		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogWarningFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(message, args);
		}

		[Conditional("LOGLEVEL_WARNING")]
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogWarningFormatWithContext(UnityEngine.Object context, string message, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(context, message, args);
		}
		#endregion

		#region INFORMATION
		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogInformation(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogInformationWithContext(object message, UnityEngine.Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogInformationFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}

		[Conditional("LOGLEVEL_INFORMATION")]
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogInformationFormatWithContext(UnityEngine.Object context, string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(context, message, args);
		}
		#endregion

		#region DEBUG
		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogDebug(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogDebugWithContext(object message, UnityEngine.Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogDebugFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}

		[Conditional("LOGLEVEL_DEBUG")]
		public static void LogDebugFormatWithContext(UnityEngine.Object context, string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(context, message, args);
		}
		#endregion
	}
}
