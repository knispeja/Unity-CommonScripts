namespace KnispelCommon.UserInterface
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// ISSUES:
	/// Doesn't really work if you need to change the text values.
	/// Also doesn't work if the screen resizes and text is on different rect transforms.
	/// Needs to watch canvas size.
	/// </summary>
	public class FontSizeBestFitMatcher : MonoBehaviour
	{
		public Text[] textList;
		private HashSet<Text> textNotToReenable;

		private bool initialized = false;
		private bool resizing = false;

		private void Awake()
		{
			textNotToReenable = new HashSet<Text>();
		}

		public void SetTextToBestFit()
		{
			if (resizing)
			{
				return;
			}

			foreach (var text in textList)
			{
				text.resizeTextMaxSize = 99999;

				if (!text.enabled)
				{
					textNotToReenable.Add(text);
				}
			}

			resizing = true;
		}

		private void Update()
		{
			if (resizing)
			{
				ApplySizes();
			}

			if (!initialized)
			{
				SetTextToBestFit();
				initialized = true;
			}
		}

		public void ApplySizes()
		{
			resizing = false;

			if (textList.Length < 0)
			{
				return;
			}

			int matchedFontSize = int.MaxValue;

			foreach (var text in textList)
			{
				if (!textNotToReenable.Contains(text))
				{
					text.enabled = true;
				}

				if (text.cachedTextGenerator.fontSizeUsedForBestFit < matchedFontSize)
				{
					matchedFontSize = UiTextUtilities.GetCurrentDynamicFontSize(text);
				}
			}

			foreach (var item in textList)
			{
				item.resizeTextMaxSize = matchedFontSize;
			}
		}
	}
}
