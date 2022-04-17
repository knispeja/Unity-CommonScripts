namespace KnispelCommon.UserInterface
{
	using KnispelCommon.Logging;
	using System;
	using UnityEngine;
	using UnityEngine.UI;

	public static class UiTextUtilities
	{
		public static int GetCurrentDynamicFontSize(Text text)
		{
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			float multiplier = 1;
			if (text.canvas)
			{
				multiplier = 1 / text.canvas.scaleFactor;
			}

			return (int)(text.cachedTextGenerator.fontSizeUsedForBestFit * multiplier);
		}

#if UNITY_EDITOR
		private static void Validate(Canvas canvas)
		{
			if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
			{
				LogWrapper.LogWarning($"{nameof(UiTextUtilities)} only work when canvas is rendering in camera space");
			}
		}
#endif

		/// <summary>
		/// Given an input Text component, clicked position in local space and UI camera returns the letter in the value of the Text component that was got clicked
		/// </summary>
		/// <param name="textComp">Text component.</param>
		/// <param name="position">Click position.</param>
		/// <param name="camera">UI camera.</param>
		/// <returns>Word in the value of the Text component that was clicked.</returns>
		public static char? FindIntersectingLetter(Text textComp, Vector3 position, Camera camera)
		{
#if UNITY_EDITOR
			Validate(textComp.canvas);
#endif

			Vector2 localPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(textComp.GetComponent<RectTransform>(),
				position, camera, out localPosition);
			int characterIndex = GetCharacterIndexFromPosition(textComp, localPosition);
			if (characterIndex >= 0 && characterIndex < textComp.text.Length)
			{
				return textComp.text[characterIndex];
			}
			return null;
		}

		/// <summary>
		/// Given an input Text component, clicked position in local space and UI camera returns the word in the value of the Text component that was got clicked
		/// </summary>
		/// <param name="textComp">Text component.</param>
		/// <param name="position">Click position.</param>
		/// <param name="camera">UI camera.</param>
		/// <returns>Word in the value of the Text component that was clicked.</returns>
		public static string FindIntersectingWord(Text textComp, Vector3 position, Camera camera)
		{
#if UNITY_EDITOR
			Validate(textComp.canvas);
#endif

			Vector2 localPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(textComp.GetComponent<RectTransform>(),
				position, camera, out localPosition);
			int characterIndex = GetCharacterIndexFromPosition(textComp, localPosition);
			if (!string.IsNullOrWhiteSpace(GetCharFromIndex(textComp, characterIndex)))
			{
				return GetWordFromCharIndex(textComp.text, characterIndex);
			}
			return "";
		}

		private static string GetCharFromIndex(Text textComp, int index)
		{
			var tempArr = textComp.text.ToCharArray();
			if (index != -1 && index < tempArr.Length)
			{
				return tempArr[index] + "";
			}
			return "";
		}

		private static int GetCharacterIndexFromPosition(Text textComp, Vector2 pos)
		{
			TextGenerator gen = textComp.cachedTextGenerator;

			if (gen.lineCount == 0)
			{
				// Sometimes the cached generator might just be out of date
				// Better hope it updates next time around...
				return 0;
			}

			int line = GetUnclampedCharacterLineFromPosition(textComp, pos, gen);
			if (line < 0)
				return -1;
			if (line >= gen.lineCount)
				return gen.characterCountVisible;

			int startCharIndex = gen.lines[line].startCharIdx;
			int endCharIndex = GetLineEndPosition(gen, line);

			for (int i = startCharIndex; i < endCharIndex; i++)
			{
				if (i >= gen.characterCountVisible)
					break;

				UICharInfo charInfo = gen.characters[i];
				Vector2 charPos = charInfo.cursorPos / textComp.pixelsPerUnit;

				if (pos.x > charPos.x && pos.x < charPos.x + (charInfo.charWidth / textComp.pixelsPerUnit))
					return i;
			}
			return endCharIndex;
		}

		private static int GetUnclampedCharacterLineFromPosition(Text textComp, Vector2 pos, TextGenerator generator)
		{
			// transform y to local scale
			float y = pos.y * textComp.pixelsPerUnit;
			float lastBottomY = 0.0f;

			for (int i = 0; i < generator.lineCount; ++i)
			{
				float topY = generator.lines[i].topY;
				float bottomY = topY - generator.lines[i].height;

				// pos is somewhere in the leading above this line
				if (y > topY)
				{
					// determine which line we're closer to
					float leading = topY - lastBottomY;
					if (y > topY - 0.5f * leading)
						return i - 1;
					else
						return i;
				}

				if (y > bottomY)
					return i;

				lastBottomY = bottomY;
			}

			// Position is after last line.
			return generator.lineCount;
		}

		private static int GetLineStartPosition(TextGenerator gen, int line)
		{
			line = Mathf.Clamp(line, 0, gen.lines.Count - 1);
			return gen.lines[line].startCharIdx;
		}

		private static int GetLineEndPosition(TextGenerator gen, int line)
		{
			line = Mathf.Max(line, 0);
			if (line + 1 < gen.lines.Count)
				return gen.lines[line + 1].startCharIdx - 1;
			return gen.characterCountVisible;
		}

		private static string GetWordFromCharIndex(string str, int characterIndex)
		{
			string firstPartOfStr = str.Substring(0, characterIndex);
			string secondPartOfStr = str.Substring(characterIndex);

			string firstPart = firstPartOfStr;
			//Check for last index of space in first part of str and get text till that
			int lastIndexOfSpace = firstPartOfStr.LastIndexOf(' ');
			if (lastIndexOfSpace != -1)
			{
				firstPart = firstPartOfStr.Substring(lastIndexOfSpace);
			}

			string secondPart = secondPartOfStr;
			//Check for first index of space in second part of str and get text till that
			int firstIndexOfSpace = secondPartOfStr.IndexOf(' ');
			if (firstIndexOfSpace != -1)
			{
				secondPart = secondPartOfStr.Substring(0, firstIndexOfSpace);
			}

			//Check for new lines in first and second parts of the word and trim it
			int IndexOfNewLineInFirstPart = firstPart.IndexOf('\n');
			if (IndexOfNewLineInFirstPart != -1)
			{
				firstPart = firstPart.Substring(firstPart.IndexOf('\n'));
			}
			int IndexOfNewLineInSecondPart = secondPart.IndexOf('\n');
			if (IndexOfNewLineInSecondPart != -1)
			{
				secondPart = secondPart.Substring(0, secondPart.IndexOf('\n'));
			}
			return firstPart.Replace("\n", "").Replace("\r", "") + secondPart.Replace("\n", "").Replace("\r", "");
		}
	}
}
