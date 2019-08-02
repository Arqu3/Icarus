// GENERATED 03/22/2019 11:16:22
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal static class UITemplateMenuItems
{
	[MenuItem("GameObject/UI/Button")]
	private static void MenuItem_Button()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Button";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Button " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("a61adc8bbcc5b8243b64e56eb54583bf")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Text")]
	private static void MenuItem_Text()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Text";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Text " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("ee9c71a61a3be8f41aef2bc71569b59d")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Toggle")]
	private static void MenuItem_Toggle()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Toggle";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Toggle " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("54f084f660711044baaa5204ea5666cf")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Slider")]
	private static void MenuItem_Slider()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Slider";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Slider " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("019d9a05b3f9a394aab0958402c6f725")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Dropdown")]
	private static void MenuItem_Dropdown()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Dropdown";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Dropdown " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("1c48e1d1d7089224694230fbfe8a06a1")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Input Field")]
	private static void MenuItem_Input_Field()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Input Field";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Input Field " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("4a80c9d66c62ca24291487a2600efb83")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

	[MenuItem("GameObject/UI/Scroll View")]
	private static void MenuItem_Scroll_View()
	{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
		string name = "Scroll View";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {
			name = "Scroll View " + i;
		}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath("2aae4d5a89454b64899edd9290bdd940")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}

}
