namespace KnispelCommon.PlayerPref
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "New Float Pref", menuName = MENU_PREFIX + "Float")]
	public class FloatPlayerPref : AbstractPlayerPreference<float>
	{
		[SerializeField]
		private float defaultValue;

		public override float GetValue()
		{
			return PlayerPrefs.GetFloat(PlayerPreferenceKey, defaultValue);
		}

		public override void SetValue(float value)
		{
			PlayerPrefs.SetFloat(PlayerPreferenceKey, value);
		}
	}
}
