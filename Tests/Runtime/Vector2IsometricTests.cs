using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Tests
{
	public class Vector2IsometricTests
	{
		private static IEnumerable<Vector2Isometric> AllIsometricVectors => Enum.GetValues(typeof(Vector2Isometric)).Cast<Vector2Isometric>();

		[Test]
		[TestCase(Vector2Isometric.UP_RIGHT, Vector2Isometric.DOWN_LEFT)]
		[TestCase(Vector2Isometric.UP_LEFT, Vector2Isometric.DOWN_RIGHT)]
		[TestCase(Vector2Isometric.DOWN_RIGHT, Vector2Isometric.UP_LEFT)]
		[TestCase(Vector2Isometric.DOWN_LEFT, Vector2Isometric.UP_RIGHT)]
		public void Reverse_IsOppositeVector(Vector2Isometric isoVector, Vector2Isometric expectedResult)
		{
			// Arrange
			// Act
			Vector2Isometric actualResult = isoVector.Reverse();

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[Test]
		[TestCaseSource(nameof(AllIsometricVectors))]
		public void Reverse_IsReversible(Vector2Isometric isoVector)
		{
			// Arrange
			Vector2Isometric expectedResult = isoVector;

			// Act
			Vector2Isometric actualResult = isoVector.Reverse().Reverse();

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[Test]
		[TestCase(Vector2Isometric.UP_LEFT, true)]
		[TestCase(Vector2Isometric.UP_RIGHT, true)]
		[TestCase(Vector2Isometric.DOWN_LEFT, false)]
		[TestCase(Vector2Isometric.DOWN_RIGHT, false)]
		public void IsFacingUp(Vector2Isometric isoVector, bool expectedResult)
		{
			// Arrange

			// Act
			bool actualResult = isoVector.IsFacingUp();

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[Test]
		[TestCase(Vector2Isometric.UP_LEFT, false)]
		[TestCase(Vector2Isometric.UP_RIGHT, true)]
		[TestCase(Vector2Isometric.DOWN_LEFT, false)]
		[TestCase(Vector2Isometric.DOWN_RIGHT, true)]
		public void IsFacingRight(Vector2Isometric isoVector, bool expectedResult)
		{
			// Arrange

			// Act
			bool actualResult = isoVector.IsFacingRight();

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}
	}
}
