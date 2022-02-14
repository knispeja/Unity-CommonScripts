namespace KnispelCommon.Graphics
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;

	[System.Serializable]
	public class RainbowColorEvent : UnityEvent<Color>
	{
	}

	public class RainbowHueShift : MonoBehaviour
	{
		[Range(0, 10)]
		public float speed = 1;

		[Range(0, 1)]
		public float saturation = 1;

		[Range(0, 1)]
		public float brightness = 1;

		[SerializeField] private RainbowColorEvent colorFunction = null;

		public Color LatestColor => Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), saturation, brightness);

		public void AddColorEventListener(Action<Color> actionOnColorChange)
		{
			if (colorFunction == null)
			{
				colorFunction = new RainbowColorEvent();
			}

			colorFunction.AddListener(new UnityAction<Color>(actionOnColorChange));
		}

		public void SetColorWhileEffectStopped(Color color)
		{
			colorFunction.Invoke(color);
		}

		private void Update()
		{
			if (speed > 0 && colorFunction != null)
			{
				colorFunction.Invoke(LatestColor);
			}
		}
	}
}
