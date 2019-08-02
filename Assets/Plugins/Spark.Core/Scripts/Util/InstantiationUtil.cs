using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstantiationUtil
{
	/// <summary>
	/// Populate the parent with a set number of copies of the object. If obj is already a child of parent,
	/// the object will be used as element 0 of the number of populated objects.
	/// </summary>
	/// <typeparam name="TObject"></typeparam>
	/// <param name="parent"></param>
	/// <param name="obj"></param>
	/// <param name="decorate"></param>
	/// <param name="times"></param>
	public static void PopulateWithObject<TObject>(this Transform parent, TObject obj, int times, System.Action<int, TObject> decorate) where TObject : Object
	{
		bool includingObj = IsChild(obj, parent);
		for (int i = includingObj ? 1 : 0; i < times; i++)
		{
			var o = Object.Instantiate(obj, parent);
			decorate(i, o);
		}
		if (includingObj)
		{
			decorate(0, obj);
		}
	}

	private static bool IsChild(Object obj, Transform parent)
	{
		var gObj = obj as GameObject;
		if (gObj)
		{
			return gObj.transform.IsChildOf(parent);
		}
		var cObj = obj as Component;
		if (cObj)
		{
			return cObj.transform.IsChildOf(parent);
		}
		return false;
	}

	public static TComponent Duplicate<TComponent>(this TComponent component) where TComponent : Component
	{
		return Object.Instantiate(component, component.transform.parent);
	}

	public static GameObject Duplicate(this GameObject gameObject)
	{
		return GameObject.Instantiate(gameObject, gameObject.transform.parent);
	}
}
