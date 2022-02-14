namespace KnispelCommon.PlayerPref
{
	using UnityEngine;

	/// <summary>
	/// Represents a single player preference name and value pair.
	/// </summary>
	/// <typeparam name="T">The type of the player preference.</typeparam>
	public abstract class AbstractPlayerPreference<T> : ScriptableObject
	{
		protected const string MENU_PREFIX = "KnispelScriptables/PlayerPref/";

		/// <summary>
		/// The <see cref="Object.name"/> of the asset is used for the preference key.
		/// </summary>
		public string PlayerPreferenceKey => name;

		/// <summary>
		/// Gets the value from player preferences.
		/// </summary>
		/// <returns>The value represented by this object.</returns>
		public abstract T GetValue();

		/// <summary>
		/// Sets the player preference value.
		/// </summary>
		/// <param name="value">A value to store in player preferences.</param>
		public abstract void SetValue(T value);

		/// <summary>
		/// Resets the player preference value by removing it from player preferences.
		/// It will appear to be the default value once again.
		/// If the player preference was already unset, this will have no effect.
		/// To reset all player preferences, use <see cref="PlayerPrefs.DeleteAll()"/>
		/// </summary>
		public void ResetValue()
		{
			PlayerPrefs.DeleteKey(PlayerPreferenceKey);
		}
	}
}
