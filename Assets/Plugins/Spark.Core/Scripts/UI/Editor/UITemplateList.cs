using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

[CreateAssetMenu(menuName = "Spark/UI Template List", fileName ="TemplateList")]
public class UITemplateList : ScriptableObject
{
	[System.Serializable]
    public struct UITemplate
	{
		public string itemName;
		public RectTransform prefab;
	}

	[SerializeField]
	private UITemplate[] templates;

	/*[InitializeOnLoadMethod]
	private static void EnsureExists()
	{
		var fullPath = Application.dataPath + "/Prefabs/UI/TemplateList.prefab";
		var localPath = "Assets/Prefabs/UI/TemplateList.prefab";
		if (!AssetDatabase.LoadAssetAtPath<UITemplateList>(localPath))
		{
			System.IO.Directory.CreateDirectory(fullPath);
			var s = ScriptableObject.CreateInstance<UITemplateList>();
			AssetDatabase.CreateAsset(s, localPath);
			AssetDatabase.Refresh();
		}
	}*/

	[ContextMenu("Refresh")]
	public void Refresh()
	{
		var templates = (from list in AssetDatabase.FindAssets("t:uITemplateList").Select(guid => AssetDatabase.LoadAssetAtPath<UITemplateList>(AssetDatabase.GUIDToAssetPath(guid)))
						from t in list.templates
						
						select new
						{
							name = t.itemName,
							guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(t.prefab))
						}).ToArray();
		var file = $@"// GENERATED {System.DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")}
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal static class UITemplateMenuItems
{{
{ 
		string.Join(System.Environment.NewLine, templates.Select(t => 
$@"	[MenuItem(""GameObject/UI/{t.name}"")]
	private static void MenuItem_{Regex.Replace(t.name, @"\s", "_")}()
	{{
		if (!Selection.activeTransform) EditorApplication.ExecuteMenuItem(""GameObject/UI/Canvas"");
		string name = ""{t.name}"";
		for(int i = 1; Selection.activeTransform.Find(name); i++) {{
			name = ""{t.name} "" + i;
		}}
		var o = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<RectTransform>(AssetDatabase.GUIDToAssetPath(""{t.guid}"")), Selection.activeGameObject.scene) as RectTransform;
		o.name = name;
		o.SetParent(Selection.activeTransform, false);
		o.anchoredPosition = Vector3.zero;
		Selection.activeObject = o;
	}}
"))  
}
}}
";
		
		System.IO.File.WriteAllText(Application.dataPath + "/Plugins/Spark.Core/Scripts/UI/Editor/UITemplateMenuItems.cs", file);
		AssetDatabase.Refresh();
	}
}
