namespace KnispelCommon.Audio
{
	using System;
	using UnityEngine;

	public static class AudioUtilities
	{
		private const float _MIN_VOLUME_DECIBELS = -80.0f;
		private const float _MAX_VOLUME_DECIBELS = 0.0f;

		public static float DecibelsToPercentVolume(float volumeDecibels)
		{
			if (volumeDecibels <= _MIN_VOLUME_DECIBELS)
			{
				return 0f;
			}

			return Mathf.Pow(10.0f, volumeDecibels / 20.0f);
		}

		public static float PercentVolumeToDecibels(float volumePercent)
		{
			if (volumePercent < 0.0f)
			{
				throw new ArgumentOutOfRangeException(nameof(volumePercent), "Percentage volume must be at or above 0.0");
			}

			if (volumePercent == 0.0f)
			{
				return _MIN_VOLUME_DECIBELS;
			}

			return Mathf.Log10(volumePercent) * 20.0f;
		}
	}
}
