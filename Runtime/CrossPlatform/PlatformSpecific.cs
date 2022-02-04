using System;
using UnityEngine;

public class PlatformSpecific : MonoBehaviour
{
	[SerializeField] private PlatformRestriction platformsToRestrictTo = PlatformRestriction.MOBILE;
	[SerializeField] private PlatformMismatchBehavior behaviorOnPlatformMismatch = PlatformMismatchBehavior.DESTROY;

    private void Start()
	{
		if (!IsPlatformRestrictionMet())
		{
			switch (behaviorOnPlatformMismatch)
			{
				case PlatformMismatchBehavior.DEACTIVATE:
					gameObject.SetActive(false);
					break;
				case PlatformMismatchBehavior.DESTROY:
					Destroy(gameObject);
					break;
				default:
					throw new InvalidOperationException(
						$"Behavior for {nameof(PlatformMismatchBehavior)}.{Enum.GetName(typeof(PlatformMismatchBehavior), platformsToRestrictTo)} not provided");
			}
		}
	}

	private bool IsPlatformRestrictionMet()
	{
		switch (platformsToRestrictTo)
		{
			case PlatformRestriction.CONSOLE:
				return Application.isConsolePlatform;
			case PlatformRestriction.DESKTOP:
				return !Application.isConsolePlatform && !Application.isMobilePlatform;
			case PlatformRestriction.EDITOR:
				return Application.isEditor;
			case PlatformRestriction.MOBILE:
				return Application.isMobilePlatform;
			default:
				throw new InvalidOperationException(
					$"Behavior for {nameof(PlatformRestriction)}.{Enum.GetName(typeof(PlatformRestriction), platformsToRestrictTo)} not provided");
		}
	}

	private enum PlatformRestriction
	{
		CONSOLE,
		DESKTOP,
		EDITOR,
		MOBILE
	}

	private enum PlatformMismatchBehavior
	{
		DEACTIVATE,
		DESTROY
	}
}
