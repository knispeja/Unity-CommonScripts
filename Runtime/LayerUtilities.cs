using System;
using UnityEngine;

public static class LayerUtilities
{
	public static void SetLayer(Renderer renderer, string name)
	{
		int sortingLayerId = SortingLayer.NameToID(name);
		if (sortingLayerId == 0)
		{
			throw new ArgumentException("Given layer name does not exist.");
		}

		renderer.sortingLayerID = sortingLayerId;
	}
}
