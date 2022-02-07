using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnityUtilities
{
	public static T FindComponentIncludeInactive<T>() where T : Component
	{
		return FindComponentsIncludeInactive<T>().FirstOrDefault();
	}

	public static IEnumerable<T> FindComponentsIncludeInactive<T>() where T : Component
	{
		return SceneManager.GetActiveScene()
			.GetRootGameObjects()
			.SelectMany(g => g.GetComponentsInChildren<T>(true));
	}

	public static Component FindComponentIncludeInactive(Type type)
	{
		return FindComponentsIncludeInactive(type).FirstOrDefault();
	}

	public static IEnumerable<Component> FindComponentsIncludeInactive(Type type)
	{
		return SceneManager.GetActiveScene()
			.GetRootGameObjects()
			.SelectMany(g => g.GetComponentsInChildren(type, true));
	}
}
