namespace KnispelCommon.PlayerPref
{
	using UnityEngine;

	/// <summary>
	/// Special player pref case because Unity does not support boolean prefs.
	/// Backed by an integer. In the case of data corruption, anything but 0 is treated as true.
	/// </summary>
	[CreateAssetMenu(fileName = "New Bool Pref", menuName = MENU_PREFIX + "Boolean")]
	public class BooleanPlayerPref : AbstractPlayerPreference<bool>
	{
		private const int FALSE_INT = 0;
		private const int TRUE_INT = 1;

		[SerializeField]
		private bool defaultValue;

		public override bool GetValue()
		{
			int defaultInt = BoolToInt(defaultValue);
			int result = PlayerPrefs.GetInt(PlayerPreferenceKey, defaultInt);
			return IntToBool(result);
		}

		public override void SetValue(bool value)
		{
			int intValue = BoolToInt(value);
			PlayerPrefs.SetInt(PlayerPreferenceKey, intValue);
		}

		private int BoolToInt(bool value)
		{
			return value ? TRUE_INT : FALSE_INT;
		}

		private bool IntToBool(int value)
		{
			// Would be easier to read 'value == TRUE_INT',
			// but this ensures anything but 0 is interpreted as 'true'
			return value != FALSE_INT;
		}
	}
}
