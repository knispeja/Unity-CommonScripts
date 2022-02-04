﻿using UnityEngine;

/// <summary>
/// Class for controlling Android mobile devices. Automatically initializes before scene is loaded.
/// Works on both legacy and >= 26 APIs, as of 3/25/2021
/// Gist link: https://gist.github.com/ruzrobert/d98220a3b7f71ccc90403e041967c46b
/// </summary>
public static class AndroidUtilities
{
    // Vibrator References
    private static AndroidJavaObject vibrator = null;
    private static AndroidJavaClass vibrationEffectClass = null;
    private static int defaultAmplitude = 255;

    // Api Level
    private static int ApiLevel = 1;
    private static bool DoesSupportVibrationEffect() => ApiLevel >= 26;    // available only from Api >= 26
    private static bool DoesSupportPredefinedEffect() => ApiLevel >= 29;   // available only from Api >= 29

    #region Initialization
#if UNITY_ANDROID
    private static bool isInitialized = false;
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
		// Line is required for Unity to add app vibration permission
#if UNITY_ANDROID
        if (Application.isConsolePlatform) { Handheld.Vibrate(); }

        // load references safely
        if (isInitialized == false && Application.platform == RuntimePlatform.Android)
        {
            // Get Api Level
            using (AndroidJavaClass androidVersionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                ApiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
            }

            // Get UnityPlayer and CurrentActivity
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (currentActivity != null)
                {
                    vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                    // if device supports vibration effects, get corresponding class
                    if (DoesSupportVibrationEffect())
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        defaultAmplitude = Mathf.Clamp(vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"), 1, 255);
                    }

                    // if device supports predefined effects, get their IDs
                    if (DoesSupportPredefinedEffect())
                    {
                        PredefinedEffect.EFFECT_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                        PredefinedEffect.EFFECT_DOUBLE_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                        PredefinedEffect.EFFECT_HEAVY_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                        PredefinedEffect.EFFECT_TICK = vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                    }
                }
            }

            isInitialized = true;
        }
#endif
	}
#endregion

#region Vibrate Public
	/// <summary>
	/// Vibrate for Milliseconds, with Amplitude (if available).
	/// If amplitude is -1, amplitude is Disabled. If -1, device DefaultAmplitude is used. Otherwise, values between 1-255 are allowed.
	/// If 'cancel' is true, Cancel() will be called automatically.
	/// </summary>
	public static void Vibrate(long milliseconds, int amplitude = -1, bool cancel = false)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string funcToStr() => string.Format("Vibrate ({0}, {1}, {2})", milliseconds, amplitude, cancel);

        Initialize(); // make sure script is initialized
        if (isInitialized == false)
        {
            Debug.LogWarning(funcToStr() + ": Not initialized");
        }
        else if (HasVibrator() == false)
        {
            Debug.LogWarning(funcToStr() + ": Device doesn't have Vibrator");
        }
        else
        {
            if (cancel) Cancel();
            if (DoesSupportVibrationEffect())
            {
                // validate amplitude
                amplitude = Mathf.Clamp(amplitude, -1, 255);
                if (amplitude == -1) amplitude = 255; // if -1, disable amplitude (use maximum amplitude)
                if (amplitude != 255 && HasAmplitudeControl() == false)
                { // if amplitude was set, but not supported, notify developer
                    Debug.LogWarning(funcToStr() + ": Device doesn't have Amplitude Control, but Amplitude was set");
                }
                if (amplitude == 0) amplitude = defaultAmplitude; // if 0, use device DefaultAmplitude

                // if amplitude is not supported, use 255; if amplitude is -1, use systems DefaultAmplitude. Otherwise use user-defined value.
                amplitude = HasAmplitudeControl() == false ? 255 : amplitude;
                VibrateEffect(milliseconds, amplitude);
            }
            else
            {
                VibrateLegacy(milliseconds);
            }
        }
#endif
    }

    /// <summary>
    /// Vibrate Pattern (pattern of durations, with format Off-On-Off-On and so on).
    /// Amplitudes can be Null (for default) or array of Pattern array length with values between 1-255.
    /// To cause the pattern to repeat, pass the index into the pattern array at which to start the repeat, or -1 to disable repeating.
    /// If 'cancel' is true, Cancel() will be called automatically.
    /// </summary>
    public static void Vibrate(long[] pattern, int[] amplitudes = null, int repeat = -1, bool cancel = false)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string funcToStr() => string.Format("Vibrate (({0}), ({1}), {2}, {3})", ArrToStr(pattern), ArrToStr(amplitudes), repeat, cancel);

        Initialize(); // make sure script is initialized
        if (isInitialized == false)
        {
            Debug.LogWarning(funcToStr() + ": Not initialized");
        }
        else if (HasVibrator() == false)
        {
            Debug.LogWarning(funcToStr() + ": Device doesn't have Vibrator");
        }
        else
        {
            // check Amplitudes array length
            if (amplitudes != null && amplitudes.Length != pattern.Length)
            {
                Debug.LogWarning(funcToStr() + ": Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.");
                amplitudes = null;
            }
            // limit amplitudes between 1 and 255
            if (amplitudes != null)
            {
                ClampAmplitudesArray(amplitudes);
            }

            // vibrate
            if (cancel) Cancel();
            if (DoesSupportVibrationEffect())
            {
                if (amplitudes != null && HasAmplitudeControl() == false)
                {
                    Debug.LogWarning(funcToStr() + ": Device doesn't have Amplitude Control, but Amplitudes was set.");
                    amplitudes = null;
                }
                if (amplitudes != null)
                {
                    VibrateEffect(pattern, amplitudes, repeat);
                }
                else
                {
                    VibrateEffect(pattern, repeat);
                }
            }
            else
            {
                VibrateLegacy(pattern, repeat);
            }
        }
#endif
    }

    /// <summary>
    /// Vibrate predefined effect (described in Vibration.PredefinedEffect). Available from Api Level >= 29.
    /// If 'cancel' is true, Cancel() will be called automatically.
    /// </summary>
    public static void VibratePredefined(int effectId, bool cancel = false)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string funcToStr() => string.Format("VibratePredefined ({0})", effectId);

        Initialize(); // make sure script is initialized
        if (isInitialized == false)
        {
            Debug.LogWarning(funcToStr() + ": Not initialized");
        }
        else if (HasVibrator() == false)
        {
            Debug.LogWarning(funcToStr() + ": Device doesn't have Vibrator");
        }
        else if (DoesSupportPredefinedEffect() == false)
        {
            Debug.LogWarning(funcToStr() + ": Device doesn't support Predefined Effects (Api Level >= 29)");
        }
        else
        {
            if (cancel) Cancel();
            VibrateEffectPredefined(effectId);
        }
#endif
    }
#endregion

#region Public Properties & Controls

    /// <summary>
    /// Returns Android Api Level
    /// </summary>
    public static int GetApiLevel() => ApiLevel;
    /// <summary>
    /// Returns Default Amplitude of device, or 0.
    /// </summary>
    public static int GetDefaultAmplitude() => defaultAmplitude;

    /// <summary>
    /// Returns true if device has vibrator
    /// </summary>
    public static bool HasVibrator()
    {
        return vibrator != null && vibrator.Call<bool>("hasVibrator");
    }
    /// <summary>
    /// Return true if device supports amplitude control
    /// </summary>
    public static bool HasAmplitudeControl()
    {
        if (HasVibrator() && DoesSupportVibrationEffect())
        {
            return vibrator.Call<bool>("hasAmplitudeControl"); // API 26+ specific
        }
        else
        {
            return false; // no amplitude control below API level 26
        }
    }

    /// <summary>
    /// Tries to cancel current vibration
    /// </summary>
    public static void Cancel()
    {
        if (HasVibrator())
        {
            vibrator.Call("cancel");
        }
    }
#endregion

#region Vibrate Internal
#region Vibration Callers
    private static void VibrateEffect(long milliseconds, int amplitude)
    {
        using (AndroidJavaObject effect = createEffect_OneShot(milliseconds, amplitude))
        {
            vibrator.Call("vibrate", effect);
        }
    }
    private static void VibrateLegacy(long milliseconds)
    {
        vibrator.Call("vibrate", milliseconds);
    }

    private static void VibrateEffect(long[] pattern, int repeat)
    {
        using (AndroidJavaObject effect = createEffect_Waveform(pattern, repeat))
        {
            vibrator.Call("vibrate", effect);
        }
    }
    private static void VibrateLegacy(long[] pattern, int repeat)
    {
        vibrator.Call("vibrate", pattern, repeat);
    }

    private static void VibrateEffect(long[] pattern, int[] amplitudes, int repeat)
    {
        using (AndroidJavaObject effect = createEffect_Waveform(pattern, amplitudes, repeat))
        {
            vibrator.Call("vibrate", effect);
        }
    }
    private static void VibrateEffectPredefined(int effectId)
    {
        using (AndroidJavaObject effect = createEffect_Predefined(effectId))
        {
            vibrator.Call("vibrate", effect);
        }
    }
#endregion

#region Vibration Effect
    /// <summary>
    /// Wrapper for public static VibrationEffect createOneShot (long milliseconds, int amplitude). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_OneShot(long milliseconds, int amplitude)
    {
        return vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
    }
    /// <summary>
    /// Wrapper for public static VibrationEffect createPredefined (int effectId). API >= 29
    /// </summary>
    private static AndroidJavaObject createEffect_Predefined(int effectId)
    {
        return vibrationEffectClass.CallStatic<AndroidJavaObject>("createPredefined", effectId);
    }
    /// <summary>
    /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int[] amplitudes, int repeat). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_Waveform(long[] timings, int[] amplitudes, int repeat)
    {
        return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, amplitudes, repeat);
    }
    /// <summary>
    /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int repeat). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_Waveform(long[] timings, int repeat)
    {
        return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, repeat);
    }
#endregion
#endregion

#region Internal
    private static string ArrToStr(long[] array) => array == null ? "null" : string.Join(", ", array);
    private static string ArrToStr(int[] array) => array == null ? "null" : string.Join(", ", array);

    private static void ClampAmplitudesArray(int[] amplitudes)
    {
        for (int i = 0; i < amplitudes.Length; i++)
        {
            amplitudes[i] = Mathf.Clamp(amplitudes[i], 1, 255);
        }
    }
#endregion

    public static class PredefinedEffect
    {
        public static int EFFECT_CLICK;         // public static final int EFFECT_CLICK
        public static int EFFECT_DOUBLE_CLICK;  // public static final int EFFECT_DOUBLE_CLICK
        public static int EFFECT_HEAVY_CLICK;   // public static final int EFFECT_HEAVY_CLICK
        public static int EFFECT_TICK;          // public static final int EFFECT_TICK
    }
}
