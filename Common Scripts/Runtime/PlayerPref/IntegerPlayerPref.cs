namespace KnispelCommon.PlayerPref
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "New Int Pref", menuName = MENU_PREFIX + "Integer")]
	public class IntegerPlayerPref : AbstractPlayerPreference<int>
	{
		[SerializeField]
		private int defaultValue;

		public override int GetValue()
		{
			return PlayerPrefs.GetInt(PlayerPreferenceKey, defaultValue);
		}

		public override void SetValue(int value)
		{
			PlayerPrefs.SetInt(PlayerPreferenceKey, value);
		}
	}
}
