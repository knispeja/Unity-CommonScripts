using System;
using NUnit.Framework;

namespace Tests
{
	public class AudioUtilitiesTests
	{
		[Test]
		[TestCase(-5)]
		public void PercentVolumeToDecibels_ThrowsArgumentOutOfRangeException(float percent)
		{
			// Arrange
			// Act, Assert
			Assert.Throws<ArgumentOutOfRangeException>(() =>
					AudioUtilities.PercentVolumeToDecibels(percent)
				);
		}

		[Test]
		[TestCase(-81)]
		public void DecibelsToPercentVolume_DoesNotThrowArgumentOutOfRangeException(float decibels)
		{
			// Arrange
			const float expectedResult = 0f;

			// Act
			float result = AudioUtilities.DecibelsToPercentVolume(decibels);

			// Assert
			Assert.AreEqual(expectedResult, result);
		}
	}
}
