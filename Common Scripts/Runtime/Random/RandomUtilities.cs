namespace KnispelCommon.Random
{
	using System;

	public static class RandomUtilities
	{
		public static T GetElement<T>(T[] elements)
		{
			return elements[UnityEngine.Random.Range(0, elements.Length)];
		}

		public static T GetElementGivenProbabilityArray<T>(T[] elements, float[] probabilities)
		{
			if (elements.Length != probabilities.Length)
			{
				throw new ArgumentException($"Length of {nameof(elements)} must match {nameof(probabilities)}");
			}

			return elements[GetIndexFromProbabilityArray(probabilities)];
		}

		public static int GetIndexFromProbabilityArray(float[] probabilities)
		{
			float total = 0;
			foreach (float element in probabilities)
			{
				total += element;
			}

			float randomPoint = UnityEngine.Random.value * total;
			for (int i = 0; i < probabilities.Length; i++)
			{
				float element = probabilities[i];
				if (randomPoint < element)
				{
					return i;
				}
				else
				{
					randomPoint -= element;
				}
			}

			// Failsafe in case Random.value returns 1
			return probabilities.Length - 1;
		}
	}
}
