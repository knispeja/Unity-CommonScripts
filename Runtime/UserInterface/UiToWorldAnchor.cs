using UnityEngine;

public class UiToWorldAnchor : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Vector2 viewportPoint;

	public static UiToWorldAnchor AddComponentTo(GameObject gameObject, Camera camera, Vector2 viewportPointToLockOnTo)
	{
		UiToWorldAnchor worldAnchor = gameObject.AddComponent<UiToWorldAnchor>();
		worldAnchor.mainCamera = camera;
		worldAnchor.viewportPoint = viewportPointToLockOnTo;
		return worldAnchor;
	}

	public void UpdateViewportPoint(Vector2 newViewportPointToLockOnTo)
	{
		viewportPoint = newViewportPointToLockOnTo;
	}

	private void Update()
	{
		Vector2 worldPoint = mainCamera.ViewportToWorldPoint(viewportPoint);
		transform.position = worldPoint;
	}
}
