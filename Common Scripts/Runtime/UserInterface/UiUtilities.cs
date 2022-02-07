using UnityEngine;
using UnityEngine.EventSystems;

public static class UiUtilities
{
	public static void CloseApplication()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	public static void HideGroup(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 0.0f;
		canvasGroup.blocksRaycasts = false;
	}

	public static void ShowGroup(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}

	public static bool IsAnyPointerOverGameObject(EventSystem eventSystem = null, bool defaultIfNoEventSystem = false)
	{
		if (eventSystem == null)
		{
			eventSystem = EventSystem.current;

			if (eventSystem == null)
			{
				return defaultIfNoEventSystem;
			}
		}

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1)
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began)
			{
				if (eventSystem.IsPointerOverGameObject(touch.fingerId))
				{
					return true;
				}
			}
		}

		return false;
#else
		return eventSystem.IsPointerOverGameObject();
#endif
	}
}
