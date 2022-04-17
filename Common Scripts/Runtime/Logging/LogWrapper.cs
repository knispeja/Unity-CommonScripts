namespace KnispelCommon.Logging
{
	using System;
	using System.Diagnostics;

	public static class LogWrapper
	{
		#region GENERIC
		public static void Log(LogLevel logLevel, object message)
		{
			switch (logLevel)
			{
				case LogLevel.NONE:
					break;
				case LogLevel.DEBUG:
					LogDebug(message);
					break;
				case LogLevel.INFORMATION:
					LogInformation(message);
					break;
				case LogLevel.WARNING:
					LogWarning(message);
					break;
				case LogLevel.ERROR:
					LogError(message);
					break;
				default:
					OnLogLevelProblemWithWrapper(logLevel);
					break;
			}
		}

		public static void LogWithContext(LogLevel logLevel, object message, UnityEngine.Object context)
		{
			switch (logLevel)
			{
				case LogLevel.NONE:
					break;
				case LogLevel.DEBUG:
					LogDebugWithContext(message, context);
					break;
				case LogLevel.INFORMATION:
					LogInformationWithContext(message, context);
					break;
				case LogLevel.WARNING:
					LogWarningWithContext(message, context);
					break;
				case LogLevel.ERROR:
					LogErrorWithContext(message, context);
					break;
				default:
					OnLogLevelProblemWithWrapper(logLevel);
					break;
			}
		}

		public static void LogFormat(LogLevel logLevel, string message, params object[] args)
		{
			switch (logLevel)
			{
				case LogLevel.NONE:
					break;
				case LogLevel.DEBUG:
					LogDebugFormat(message, args);
					break;
				case LogLevel.INFORMATION:
					LogInformationFormat(message, args);
					break;
				case LogLevel.WARNING:
					LogWarningFormat(message, args);
					break;
				case LogLevel.ERROR:
					LogErrorFormat(message, args);
					break;
				default:
					OnLogLevelProblemWithWrapper(logLevel);
					break;
			}
		}

		public static void LogFormatWithContext(LogLevel logLevel, UnityEngine.Object context, string message, params object[] args)
		{
			switch (logLevel)
			{
				case LogLevel.NONE:
					break;
				case LogLevel.DEBUG:
					LogDebugFormatWithContext(message, context, args);
					break;
				case LogLevel.INFORMATION:
					LogInformationFormatWithContext(message, context, args);
					break;
				case LogLevel.WARNING:
					LogWarningFormatWithContext(message, context, args);
					break;
				case LogLevel.ERROR:
					LogErrorFormatWithContext(message, context, args);
					break;
				default:
					OnLogLevelProblemWithWrapper(logLevel);
					break;
			}
		}

		private static void OnLogLevelProblemWithWrapper(LogLevel logLevel)
		{
			LogErrorFormat(
					"Log level {0} not supported by log wrapper. Something went catastrophically wrong.",
					Enum.GetName(typeof(LogLevel), logLevel)
				);
		}

		#endregion

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
		public static void LogErrorFormatWithContext(string message, UnityEngine.Object context, params object[] args)
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
		public static void LogWarningFormatWithContext(string message, UnityEngine.Object context, params object[] args)
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
		public static void LogInformationFormatWithContext(string message, UnityEngine.Object context, params object[] args)
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
		public static void LogDebugFormatWithContext(string message, UnityEngine.Object context, params object[] args)
		{
			UnityEngine.Debug.LogFormat(context, message, args);
		}
		#endregion
	}
}
