using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Linq;
namespace Spark.Localization
{
	public class FindLocalizationId : EditorWindow
	{
		private struct Entry
		{
			public string value;
			public string description;
		}

		private string locId;

		[MenuItem("Tools/Find Localization Id")]
		private static void DoFindLocalizationId()
		{
			var win = EditorWindow.GetWindow(typeof(FindLocalizationId), false, null, true);
			win.Show(true);
		}

		void OnGUI()
		{
			locId = EditorGUILayout.TextField("Paste Id here", locId).Trim();
			GUILayout.Space(30);
			if (GUILayout.Button("Find") && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				// Grab all localized fields in resources
				foreach (var res in Resources.LoadAll<GameObject>(""))
				{
					if (res.name.EndsWith("Canvas")) continue;
					foreach (var comp in res.GetComponentsInChildren<MonoBehaviour>(true))
					{
						//Debug.Log(comp);
						if (ProcessObject(comp, null))
						{
							var t = comp.transform;
							while (t.parent != null) t = t.parent;

							Selection.activeObject = t;
							EditorGUIUtility.PingObject(t);
							return;
						}
					}
				}

				foreach (var uiScene in AssetDatabase.FindAssets("t:scene", new[] { "Assets/Scenes/UI" }))
				{
					var path = AssetDatabase.GUIDToAssetPath(uiScene);
					var scene = EditorSceneManager.OpenScene(path);
					foreach (var obj in scene.GetRootGameObjects())
					{
						foreach (var comp in obj.GetComponentsInChildren<MonoBehaviour>(true))
						{
							if (ProcessObject(comp, null))
							{
								return;
							}
						}
					}
				}

				for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
				{
					var bScene = EditorBuildSettings.scenes[i];
					var scene = EditorSceneManager.OpenScene(bScene.path);
					foreach (var obj in scene.GetRootGameObjects())
					{
						foreach (var comp in obj.GetComponentsInChildren<MonoBehaviour>(true))
						{
							if (ProcessObject(comp, null))
							{
								return;
							}
						}
					}
				}


			}

		}

		private bool ProcessObject(Object obj, Dictionary<string, Entry> dict, bool isResource = false)
		{
			var serializedObj = new SerializedObject(obj);
			var property = serializedObj.GetIterator();
			while (property.Next(true))
			{
				// Resolve the type
				var type = property.serializedObject.targetObject.GetType();
				var field = GetFieldViaPath(type, property.propertyPath);
				if (field != null && field.FieldType.Equals(typeof(LocalizedString)))
				{
					// LocalizedString
					var val = field.GetValue(obj) as LocalizedString;
					Debug.Log(val.id + " = " + locId);
					if (!string.IsNullOrEmpty(val.id) && val.id.Equals(locId))
					{
						Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " : " + obj.name);
						Selection.activeObject = obj;
						EditorGUIUtility.PingObject(obj);
						return true;
					}
				}
				else if (type.Equals(typeof(LocalizedText)))
				{
					// LocalizedText
					var o = property.serializedObject.targetObject as LocalizedText;
					if (!string.IsNullOrEmpty(o.id) && o.id.Equals(locId))
					{
						Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " : " + obj.name);
						Selection.activeObject = obj;
						EditorGUIUtility.PingObject(obj);
						return true;
					}
				}
			}
			return false;
		}

		public static System.Reflection.FieldInfo GetFieldViaPath(System.Type type, string path)
		{
			System.Type parentType = type;
			System.Reflection.FieldInfo fi = type.GetField(path);
			string[] perDot = path.Split('.');
			foreach (string fieldName in perDot)
			{
				fi = parentType.GetField(fieldName);
				if (fi != null)
					parentType = fi.FieldType;
				else
					return null;
			}
			if (fi != null)
				return fi;
			else return null;
		}
	}
}

