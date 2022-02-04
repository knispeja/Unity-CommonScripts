using UnityEngine;

/// <summary>
/// Helper methods for setting layers on a renderer.
/// Unlike directly setting <see cref="Renderer.sortingLayerName"/> or <see cref="Renderer.sortingLayerID"/>,
/// these methods log warnings if the layer is invalid and will not change the layer.
/// </summary>
public static class LayerUtilities
{
	public static bool TrySetLayer(Renderer renderer, string layerName)
	{
		int sortingLayerId = SortingLayer.NameToID(layerName);
		if (sortingLayerId == 0)
		{
			Debug.LogWarning($"Provided layer name '{layerName}' is not valid.");
			return false;
		}

		renderer.sortingLayerID = sortingLayerId;
		return true;
	}

	public static bool TrySetLayer(Renderer renderer, int layerId)
	{
		if (!SortingLayer.IsValid(layerId))
		{
			Debug.LogWarning($"Provided layer ID '{layerId}' is not valid.");
			return false;
		}

		renderer.sortingLayerID = layerId;
		return true;
	}
}
