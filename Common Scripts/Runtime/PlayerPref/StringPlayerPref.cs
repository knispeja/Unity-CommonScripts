namespace KnispelCommon.PlayerPref
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "New String Pref", menuName = MENU_PREFIX + "String")]
	public class StringPlayerPref : AbstractPlayerPreference<string>
	{
		[SerializeField]
		private string defaultValue;

		public override string GetValue()
		{
			return PlayerPrefs.GetString(PlayerPreferenceKey, defaultValue);
		}

		public override void SetValue(string value)
		{
			PlayerPrefs.SetString(PlayerPreferenceKey, value);
		}
	}
}
