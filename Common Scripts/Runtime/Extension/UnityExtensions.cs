namespace KnispelCommon.Extension
{
	using System.Collections;
	using UnityEngine;

	public static class UnityExtensions
	{
		// Do not call SetActive methods with delays on the same object multiple times before delay has been reached and active state has changed
		public static void SetActive(this MonoBehaviour monoBehaviour, bool value, float delaySeconds)
		{
			monoBehaviour.gameObject.SetActive(value, monoBehaviour, delaySeconds);
		}

		public static void SetActive(this GameObject gameObject, bool value, MonoBehaviour coroutineContext, float delaySeconds)
		{
			coroutineContext.StartCoroutine(
					SetActiveLate(gameObject, value, delaySeconds)
				);
		}

		private static IEnumerator SetActiveLate(GameObject gameObject, bool value, float delaySeconds)
		{
			yield return new WaitForSeconds(delaySeconds);
			gameObject.SetActive(value);
		}
	}
}
