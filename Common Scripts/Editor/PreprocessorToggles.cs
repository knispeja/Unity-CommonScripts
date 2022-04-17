using UnityEditor;

[InitializeOnLoad]
public static class PreprocessorToggles
{
	private const string LOGLEVEL_NONE = nameof(LOGLEVEL_NONE);
	private const string LOGLEVEL_ERROR = nameof(LOGLEVEL_ERROR);
	private const string LOGLEVEL_WARNING = nameof(LOGLEVEL_WARNING);
	private const string LOGLEVEL_INFORMATION = nameof(LOGLEVEL_INFORMATION);
	private const string LOGLEVEL_DEBUG = nameof(LOGLEVEL_DEBUG);

	private const string MENU_LOGLEVEL_PREFIX = "Logging/";
	private const string MENU_LOGLEVEL_DISABLED = MENU_LOGLEVEL_PREFIX + "Disabled";
	private const string MENU_LOGLEVEL_ERROR = MENU_LOGLEVEL_PREFIX + "Error";
	private const string MENU_LOGLEVEL_WARNING = MENU_LOGLEVEL_PREFIX + "Warning";
	private const string MENU_LOGLEVEL_INFORMATION = MENU_LOGLEVEL_PREFIX + "Information";
	private const string MENU_LOGLEVEL_DEBUG = MENU_LOGLEVEL_PREFIX + "Debug";

	private const string DIRECTIVE_ANALYTICS_DISABLED = "DISABLE_ANALYTICS";
	private const string MENU_ANALYTICS_DISABLED = "Analytics/Full Disable (overrides all)";

	private const string DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR = "ENABLE_ANALYTICS_IN_EDITOR";
	private const string MENU_ANALYTICS_ENABLED_IN_EDITOR = "Analytics/Enable In Editor";

	private const string DIRECTIVE_ANALYTICS_LOG_IN_EDITOR = "LOG_ANALYTICS_EVENTS_IN_EDITOR";
	private const string MENU_ANALYTICS_LOG_IN_EDITOR = "Analytics/Log Events in Editor";

	private static string logLevel;
	private static bool analyticsDisabled;
	private static bool analyticsEnabledInEditor;
	private static bool analyticsLogInEditor;

	private static string[] LogLevels => new[] { LOGLEVEL_NONE, LOGLEVEL_ERROR, LOGLEVEL_WARNING, LOGLEVEL_INFORMATION, LOGLEVEL_DEBUG };

	static PreprocessorToggles()
	{
		analyticsDisabled = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_DISABLED);
		analyticsEnabledInEditor = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR);
		analyticsLogInEditor = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_LOG_IN_EDITOR);

		logLevel = LOGLEVEL_DEBUG; // Default log level
		foreach (string level in LogLevels)
		{
			if (GlobalDefineUtilities.IsDirectiveDefined(level))
			{
				logLevel = level;
				break;
			}
		}

		// Delay until first editor tick so menu is populated before setting check state
		EditorApplication.delayCall += () =>
		{
			AfterToggle(analyticsDisabled,			MENU_ANALYTICS_DISABLED,			DIRECTIVE_ANALYTICS_DISABLED);
			AfterToggle(analyticsEnabledInEditor,	MENU_ANALYTICS_ENABLED_IN_EDITOR,	DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR);
			AfterToggle(analyticsLogInEditor,		MENU_ANALYTICS_LOG_IN_EDITOR,		DIRECTIVE_ANALYTICS_LOG_IN_EDITOR);

			AfterLogLevelToggle();
		};
	}

	[MenuItem(MENU_ANALYTICS_DISABLED)]
	private static void ToggleAnalyticsDisabled()
	{
		analyticsDisabled = !analyticsDisabled;
		AfterToggle(analyticsDisabled, MENU_ANALYTICS_DISABLED, DIRECTIVE_ANALYTICS_DISABLED);
	}

	[MenuItem(MENU_ANALYTICS_ENABLED_IN_EDITOR)]
	private static void ToggleAnalyticsEnabledInEditor()
	{
		analyticsEnabledInEditor = !analyticsEnabledInEditor;
		AfterToggle(analyticsEnabledInEditor, MENU_ANALYTICS_ENABLED_IN_EDITOR, DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR);
	}

	[MenuItem(MENU_ANALYTICS_LOG_IN_EDITOR)]
	private static void ToggleAnalyticsLogInEditor()
	{
		analyticsLogInEditor = !analyticsLogInEditor;
		AfterToggle(analyticsLogInEditor, MENU_ANALYTICS_LOG_IN_EDITOR, DIRECTIVE_ANALYTICS_LOG_IN_EDITOR);
	}

	[MenuItem(MENU_LOGLEVEL_DISABLED)]
	private static void SetLogsDisabledInEditor()
	{
		logLevel = LOGLEVEL_NONE;
		AfterLogLevelToggle();
	}

	[MenuItem(MENU_LOGLEVEL_ERROR)]
	private static void SetLogsErrorInEditor()
	{
		logLevel = LOGLEVEL_ERROR;
		AfterLogLevelToggle();
	}

	[MenuItem(MENU_LOGLEVEL_WARNING)]
	private static void SetLogsWarningInEditor()
	{
		logLevel = LOGLEVEL_WARNING;
		AfterLogLevelToggle();
	}

	[MenuItem(MENU_LOGLEVEL_INFORMATION)]
	private static void SetLogsInformationInEditor()
	{
		logLevel = LOGLEVEL_INFORMATION;
		AfterLogLevelToggle();
	}

	[MenuItem(MENU_LOGLEVEL_DEBUG)]
	private static void SetLogsDebugInEditor()
	{
		logLevel = LOGLEVEL_DEBUG;
		AfterLogLevelToggle();
	}

	private static void AfterLogLevelToggle()
	{
		AfterToggle(logLevel == LOGLEVEL_NONE,			MENU_LOGLEVEL_DISABLED,		LOGLEVEL_NONE);
		AfterToggle(logLevel == LOGLEVEL_ERROR,			MENU_LOGLEVEL_ERROR,		LOGLEVEL_ERROR);
		AfterToggle(logLevel == LOGLEVEL_WARNING,		MENU_LOGLEVEL_WARNING,		LOGLEVEL_WARNING);
		AfterToggle(logLevel == LOGLEVEL_INFORMATION,	MENU_LOGLEVEL_INFORMATION,	LOGLEVEL_INFORMATION);
		AfterToggle(logLevel == LOGLEVEL_DEBUG,			MENU_LOGLEVEL_DEBUG,		LOGLEVEL_DEBUG);
	}

	private static void AfterToggle(bool enabled, string menuName, string directiveName)
	{
		Menu.SetChecked(menuName, enabled);
		if (enabled)
		{
			GlobalDefineUtilities.DefineDirectiveIfNotDefined(directiveName);
		}
		else
		{
			GlobalDefineUtilities.RemoveDirectiveIfDefined(directiveName);
		}
	}
}
