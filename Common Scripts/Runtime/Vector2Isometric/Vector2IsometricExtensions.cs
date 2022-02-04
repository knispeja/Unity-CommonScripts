using UnityEngine;

public static class Vector2IsometricExtensions
{
	/// <summary>
	/// Use this to configure the X / Y ratio for the grid
	/// Default value results in a 90 / 63.435 / 26.565 degree triangle
	/// </summary>
	public const float ISOMETRIC_RATIO = 2f;

	/// <summary>
	/// These two constants will meet the following criteria:
	///		1. X + Y = 1.0
	///		2. X / Y = <see cref="ISOMETRIC_RATIO"/>
	/// </summary>
	private const float ISOMETRIC_X = ISOMETRIC_RATIO / (ISOMETRIC_RATIO + 1f);
	private const float ISOMETRIC_Y = 1f / (ISOMETRIC_RATIO + 1f);

	private static readonly Vector2 upRight = new Vector2(ISOMETRIC_X, ISOMETRIC_Y).normalized;
	private static readonly Vector2 upLeft = new Vector2(-ISOMETRIC_X, ISOMETRIC_Y).normalized;
	private static readonly Vector2 downRight = new Vector2(ISOMETRIC_X, -ISOMETRIC_Y).normalized;
	private static readonly Vector2 downLeft = new Vector2(-ISOMETRIC_X, -ISOMETRIC_Y).normalized;

	public static bool TryConvert(this Vector2 normlizedVector, out Vector2Isometric isometricVector)
	{
		bool success = true;
		if (normlizedVector == upRight)
		{
			isometricVector = Vector2Isometric.UP_RIGHT;
		}
		else if (normlizedVector == upLeft)
		{
			isometricVector = Vector2Isometric.UP_LEFT;
		}
		else if (normlizedVector == downRight)
		{
			isometricVector = Vector2Isometric.DOWN_RIGHT;
		}
		else if (normlizedVector == downLeft)
		{
			isometricVector = Vector2Isometric.DOWN_LEFT;
		}
		else
		{
			isometricVector = Vector2Isometric.DOWN_LEFT;
			success = false;
		}

		return success;
	}

	public static Vector2 ToVector2(this Vector2Isometric isometricVector)
	{
		switch (isometricVector)
		{
			case Vector2Isometric.UP_RIGHT:
				return upRight;
			case Vector2Isometric.UP_LEFT:
				return upLeft;
			case Vector2Isometric.DOWN_RIGHT:
				return downRight;
			default:
				return downLeft;
		}
	}

	public static bool IsFacingUp(this Vector2Isometric isometricVector)
	{
		return 
			isometricVector == Vector2Isometric.UP_LEFT || 
			isometricVector == Vector2Isometric.UP_RIGHT;
	}

	public static bool IsFacingRight(this Vector2Isometric isometricVector)
	{
		return
			isometricVector == Vector2Isometric.UP_RIGHT ||
			isometricVector == Vector2Isometric.DOWN_RIGHT;
	}

	public static Vector2Isometric Reverse(this Vector2Isometric isometricVector)
	{
		switch (isometricVector)
		{
			case Vector2Isometric.UP_RIGHT:
				return Vector2Isometric.DOWN_LEFT;
			case Vector2Isometric.UP_LEFT:
				return Vector2Isometric.DOWN_RIGHT;
			case Vector2Isometric.DOWN_RIGHT:
				return Vector2Isometric.UP_LEFT;
			default:
				return Vector2Isometric.UP_RIGHT;
		}
	}

	public static Vector2Isometric RotateClockwise(this Vector2Isometric isometricVector)
	{
		switch (isometricVector)
		{
			case Vector2Isometric.UP_RIGHT:
				return Vector2Isometric.DOWN_RIGHT;
			case Vector2Isometric.UP_LEFT:
				return Vector2Isometric.UP_RIGHT;
			case Vector2Isometric.DOWN_RIGHT:
				return Vector2Isometric.DOWN_LEFT;
			default:
				return Vector2Isometric.UP_LEFT;
		}
	}

	public static Vector2Isometric RotateCounterclockwise(this Vector2Isometric isometricVector)
	{
		switch (isometricVector)
		{
			case Vector2Isometric.UP_RIGHT:
				return Vector2Isometric.UP_LEFT;
			case Vector2Isometric.UP_LEFT:
				return Vector2Isometric.DOWN_LEFT;
			case Vector2Isometric.DOWN_RIGHT:
				return Vector2Isometric.UP_RIGHT;
			default:
				return Vector2Isometric.DOWN_RIGHT;
		}
	}
}
