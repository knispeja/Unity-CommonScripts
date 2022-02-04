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
		public void DecibelsToPercentVolume_ThrowsArgumentOutOfRangeException(float decibels)
		{
			// Arrange
			// Act, Assert
			Assert.Throws<ArgumentOutOfRangeException>(() =>
					AudioUtilities.DecibelsToPercentVolume(decibels)
				);
		}
	}
}
