using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Reflection;

namespace Spark.Localization
{
	/// <summary>
	/// Generates a .csv file for localized text in the game.
	/// Supports:
	/// * strings in MonoBehaviours
	/// * UI Text
	/// * Objects in scenes and resources
	/// * Prefab linking
	/// Doesn't support:
	/// * ScriptableObject assets
	/// * Audio
	/// * Textures
	/// </summary>
	public static class LocalizationGenerator
	{

		private struct Entry
		{
			public string value;
			public string description;
		}

		[MenuItem("Tools/Generate Localization")]
		public static void GenerateLocalization()
		{
			try
			{
				// GO through each prefab and each scene in build settings and acquire guids
				// if a field doesn't have a guid, generate it

				var dict = new Dictionary<string, Entry>();


				EditorUtility.DisplayProgressBar("", "", 0);
				EditorSceneManager.SaveOpenScenes();

				for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
				{
					var bScene = EditorBuildSettings.scenes[i];
					var scene = EditorSceneManager.OpenScene(bScene.path);
					foreach (var obj in scene.GetRootGameObjects())
					{
						foreach (var comp in obj.GetComponentsInChildren<MonoBehaviour>(true))
						{
							ProcessObject(comp, dict);
						}
					}
					EditorSceneManager.SaveOpenScenes();
				}

				foreach (var scriptable in AssetDatabase.FindAssets("t:scriptableObject", new[] { "Assets" }))
				{
					var o = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(scriptable));
					ProcessObject(o, dict);
				}

				// Grab all localized fields in resources
				foreach (var res in Resources.LoadAll<GameObject>(""))
				{
					foreach (var comp in res.GetComponentsInChildren<MonoBehaviour>(true))
					{
						//Debug.Log(comp);
						ProcessObject(comp, dict);
					}
				}

				WriteCsv(dict);
				EditorUtility.ClearProgressBar();
			}
			catch (System.Exception e)
			{
				EditorUtility.ClearProgressBar();
				throw e;
			}
		}

		private static void WriteCsv(Dictionary<string, Entry> dict)
		{
			using (var stream = new StreamWriter(new FileStream(Application.dataPath + "/Localization/gen.csv", FileMode.Create)))
			{
				stream.WriteLine(@"""Identifier"",""English"",""Description""");
				foreach (var v in dict)
				{
					stream.WriteLine(string.Format(@"""{0}"",""{1}"",""{2}""", v.Key, v.Value.value, v.Value.description));
				}
			}
			AssetDatabase.Refresh();
		}

		private static void ProcessObject(Object obj, Dictionary<string, Entry> dict)
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
					if (string.IsNullOrEmpty(val.id))
					{
						// ensure the id is valid, or else generate one
						var p = property.FindPropertyRelative("id");
						p.stringValue = System.Guid.NewGuid().ToString();
						p.serializedObject.ApplyModifiedPropertiesWithoutUndo();
						EditorSceneManager.MarkAllScenesDirty();
					}
					if (!dict.ContainsKey(val.id))
					{
						dict.Add(val.id, new Entry { value = val.baseValue, description = val.description });

					}
				}
				else if (field != null && field.FieldType.Equals(typeof(LocalizedString[])))
				{
					var vals = field.GetValue(obj) as LocalizedString[];
					foreach (var val in vals)
					{
						if (string.IsNullOrEmpty(val.id))
						{
							// ensure the id is valid, or else generate one
							var p = property.FindPropertyRelative("id");
							p.stringValue = System.Guid.NewGuid().ToString();
							p.serializedObject.ApplyModifiedPropertiesWithoutUndo();
							EditorSceneManager.MarkAllScenesDirty();
						}
						if (!dict.ContainsKey(val.id))
						{
							dict.Add(val.id, new Entry { value = val.baseValue, description = val.description });

						}
					}
				}
				else if (type.Equals(typeof(LocalizedText)))
				{
					// LocalizedText
					var o = property.serializedObject.targetObject as LocalizedText;
					if (string.IsNullOrEmpty(o.id))
					{
						// ensure the id is valid, or else generate one
						o.id = System.Guid.NewGuid().ToString();
					}
					property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
					EditorSceneManager.MarkAllScenesDirty();
					if (!dict.ContainsKey(o.id))
					{
						dict.Add(o.id, new Entry { value = o.GetComponent<UnityEngine.UI.Text>().text, description = o.description });
					}
				}
			}
		}

		private static void ProcessLocalizedString(FieldInfo field, SerializedProperty property, Object obj, Dictionary<string, Entry> dict)
		{

		}


		public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type, string path)
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