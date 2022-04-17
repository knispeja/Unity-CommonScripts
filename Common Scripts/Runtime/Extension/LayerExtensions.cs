namespace KnispelCommon.Extension
{
	using KnispelCommon.Logging;
	using UnityEngine;

	/// <summary>
	/// Helper methods for setting layers on a renderer.
	/// Unlike directly setting <see cref="Renderer.sortingLayerName"/> or <see cref="Renderer.sortingLayerID"/>,
	/// these methods log warnings if the layer is invalid and will not change the layer.
	/// </summary>
	public static class LayerExtensions
	{
		public static bool TrySetLayer(this Renderer renderer, string layerName)
		{
			int sortingLayerId = SortingLayer.NameToID(layerName);
			if (sortingLayerId == 0)
			{
				LogWrapper.LogWarningFormat("Provided layer name '{0}' is not valid.", layerName);
				return false;
			}

			renderer.sortingLayerID = sortingLayerId;
			return true;
		}

		public static bool TrySetLayer(this Renderer renderer, int layerId)
		{
			if (!SortingLayer.IsValid(layerId))
			{
				LogWrapper.LogWarningFormat("Provided layer ID '{0}' is not valid.", layerId);
				return false;
			}

			renderer.sortingLayerID = layerId;
			return true;
		}
	}
}
