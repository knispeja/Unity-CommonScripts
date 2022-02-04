using System;
using UnityEditor;

public static class GlobalDefineUtilities
{
	public static bool IsDirectiveDefined(string directive)
	{
		return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains(directive);
	}

	public static void DefineDirectiveIfNotDefined(string directive)
	{
		if (string.IsNullOrWhiteSpace(directive))
		{
			throw new ArgumentException(nameof(directive), "Bad directive name");
		}

		string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
		if (!defines.Contains(directive))
		{
			if (defines.Length > 0)
			{
				defines += ";";
			}

			defines += directive;

			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
		}
	}

	public static void RemoveDirectiveIfDefined(string directive)
	{
		if (string.IsNullOrWhiteSpace(directive))
		{
			throw new ArgumentException(nameof(directive), "Bad directive name");
		}

		string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
		if (defines.StartsWith(directive))
		{
			defines = defines.Replace(directive, string.Empty);
		}
		else
		{
			defines = defines.Replace(";" + directive, string.Empty);
		}

		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
	}
}
