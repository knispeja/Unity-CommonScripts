using UnityEditor;

[InitializeOnLoad]
public static class PreprocessorToggles
{
	private const string DIRECTIVE_ANALYTICS_DISABLED = "DISABLE_ANALYTICS";
	private const string MENU_ANALYTICS_DISABLED = "Analytics/Full Disable (overrides all)";

	private const string DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR = "ENABLE_ANALYTICS_IN_EDITOR";
	private const string MENU_ANALYTICS_ENABLED_IN_EDITOR = "Analytics/Enable In Editor";

	private const string DIRECTIVE_ANALYTICS_LOG_IN_EDITOR = "LOG_ANALYTICS_EVENTS_IN_EDITOR";
	private const string MENU_ANALYTICS_LOG_IN_EDITOR = "Analytics/Log Events in Editor";

	private static bool analyticsDisabled;
	private static bool analyticsEnabledInEditor;
	private static bool analyticsLogInEditor;

	static PreprocessorToggles()
	{
		analyticsDisabled = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_DISABLED);
		analyticsEnabledInEditor = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR);
		analyticsLogInEditor = GlobalDefineUtilities.IsDirectiveDefined(DIRECTIVE_ANALYTICS_LOG_IN_EDITOR);

		// Delay until first editor tick so menu is populated before setting check state
		EditorApplication.delayCall += () =>
		{
			AfterToggle(analyticsDisabled, MENU_ANALYTICS_DISABLED, DIRECTIVE_ANALYTICS_DISABLED);
			AfterToggle(analyticsEnabledInEditor, MENU_ANALYTICS_ENABLED_IN_EDITOR, DIRECTIVE_ANALYTICS_ENABLED_IN_EDITOR);
			AfterToggle(analyticsLogInEditor, MENU_ANALYTICS_LOG_IN_EDITOR, DIRECTIVE_ANALYTICS_LOG_IN_EDITOR);
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
