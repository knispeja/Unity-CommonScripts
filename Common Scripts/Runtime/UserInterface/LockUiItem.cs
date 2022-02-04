using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LockUiItem : MonoBehaviour
{
	[SerializeField] private bool locked = true;

	private RectTransform rectTransform;
	private Vector3 originalPosition;

	public bool Locked
	{
		get
		{
			return locked;
		}
		set
		{
			locked = value;
			originalPosition = rectTransform.position;
		}
	}

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		originalPosition = rectTransform.position;
	}

	private void Update()
	{
		if (locked && originalPosition != rectTransform.position)
		{
			gameObject.GetComponent<RectTransform>().position = originalPosition;
		}
	}
}
