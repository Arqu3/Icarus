using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;

public class GameTweaker : EditorWindow
{

	private List<TweakableClass> tweakables;
	private Vector2 scroll = Vector2.zero;

	private struct TweakableClass
	{
		// Class type
		public Type type;
		public bool hasPrefab;
		// Tweakable fields
		public FieldInfo[] sharedFields;
		public FieldInfo[] instancedFields;

		public UnityEngine.Object[] objects;
		// gameobjects in scene with script of type
		// public SerializedObject[] objects;

	}

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Game Tweaker")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		GameTweaker window = (GameTweaker)EditorWindow.GetWindow(typeof(GameTweaker));
		window.RefreshContent();
		window.Show();
	}

	private bool AssemblyNameLookedAt(string assemblyName)
	{
		string[] assemblies = { "Assembly-CSharp-firstpass", "Assembly-CSharp" };
		bool exists = false;
		foreach (string s in assemblies)
		{
			exists |= s.Equals(assemblyName);
		}
		return exists;
	}

	/// <summary>
	/// Performs a new scan for tweakable objects.
	/// </summary>
	private void RefreshContent()
	{
		tweakables = new List<TweakableClass>();
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			if (AssemblyNameLookedAt(assembly.GetName().Name))
				foreach (Type type in assembly.GetTypes())
				{
					List<FieldInfo> sharedFields = new List<FieldInfo>();
					List<FieldInfo> instancedFields = new List<FieldInfo>();
					foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						TweakableFieldAttribute tweakableAttribute = null;
						bool isSerialized = field.IsPublic;
						foreach (Attribute att in field.GetCustomAttributes(true))
						{
							if (att is TweakableFieldAttribute) tweakableAttribute = att as TweakableFieldAttribute;
							isSerialized |= att is SerializeField;
							if (tweakableAttribute != null && isSerialized)
							{
								if (tweakableAttribute.isSharedAmongAllInstances)
								{
									sharedFields.Add(field);
								}
								else
								{
									instancedFields.Add(field);
								}
								break;
							}
						}
						if (tweakableAttribute != null && !isSerialized)
						{
							Debug.LogWarning("It appears some tweakable fields aren't seralized. This is probably due to the fields being marked as private without having the [SerializeField] attribute.");
						}
					}
					if (sharedFields.Count > 0 || instancedFields.Count > 0)
					{
						TweakableClass c = new TweakableClass();
						c.type = type;
						c.sharedFields = sharedFields.ToArray();
						c.instancedFields = instancedFields.ToArray();
						c.objects = Resources.FindObjectsOfTypeAll(type);
						UnityEngine.Object obj;
						if (c.objects.Length > 0 && (obj = PrefabUtility.GetCorrespondingObjectFromSource(c.objects[0])) != null)
						{
							c.hasPrefab = true;
							for (int i = 0; i < c.objects.Length; i++)
							{
								if (c.objects[i] == obj)
								{
									var tObj = c.objects[0];
									c.objects[0] = c.objects[i];
									c.objects[i] = tObj;
									break;
								}
							}
						}
						else
						{
							c.hasPrefab = false;
						}
						tweakables.Add(c);
					}
				}
		}
	}

	void OnGUI()
	{
		try
		{
			scroll = EditorGUILayout.BeginScrollView(scroll);

			foreach (TweakableClass c in tweakables)
			{
				if (c.objects.Length > 0)
				{
					EditorGUILayout.LabelField(c.type.ToString(), EditorStyles.boldLabel);
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Shared Settings", EditorStyles.boldLabel);
					EditorGUI.indentLevel++;
					SerializedObject o = new SerializedObject(c.objects[0]);

					List<SerializedProperty> props = new List<SerializedProperty>();
					foreach (FieldInfo field in c.sharedFields)
					{

						SerializedProperty p = o.FindProperty(field.Name);
						if (p == null)
						{
							Debug.LogWarning("non-properties aren't supported yet. The type: (" + field.FieldType + ") of \"" + c.type + "." + field.Name + "\" is currently illegal.");
						}
						else
						{
							PropertyField(p);
							props.Add(p);
						}


					}
					o.ApplyModifiedProperties();
					EditorGUI.indentLevel--;


					foreach (UnityEngine.Object obj in c.objects)
					{
						EditorGUILayout.LabelField(obj.name, EditorStyles.boldLabel);
						EditorGUI.indentLevel++;
						SerializedObject instObj = new SerializedObject(obj);
						foreach (SerializedProperty p in props) // copy shared props
						{
							instObj.CopyFromSerializedProperty(p);
						}
						instObj.ApplyModifiedPropertiesWithoutUndo();

						GUI.changed = false;
						foreach (FieldInfo field in c.instancedFields)
						{
							SerializedProperty prop = instObj.FindProperty(field.Name);
							if (prop == null)
							{
								Debug.LogWarning("non-properties aren't supported yet. The type: (" + field.FieldType + ") of \"" + c.type + "." + field.Name + "\" is currently illegal.");
							}
							else
								PropertyField(prop);

						}

						instObj.ApplyModifiedProperties();
						UnityEngine.Object objParent;
						if (GUI.changed && (objParent = PrefabUtility.GetCorrespondingObjectFromSource(obj)) != null && objParent != obj)
						{
							PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
						}
						GUI.changed = false;


						EditorGUI.indentLevel--;
					}

					EditorGUI.indentLevel--;
				}
				EditorGUILayout.Separator();

			}
			EditorGUILayout.EndScrollView();
		}
		catch (UnityException e)
		{
			Debug.LogWarning("error occured, refreshing...\n" + e.Message);
			RefreshContent();
		}

	}

	/// <summary>
	/// helper function to automatically handle arrays
	/// </summary>
	/// <param name="prop"></param>
	private void PropertyField(SerializedProperty prop)
	{

		if (prop.isArray)
		{
			DrawArrayProperty(prop);
		}
		else
		{
			EditorGUILayout.PropertyField(prop);
			if (prop.isInstantiatedPrefab)
			{
				var preProp = new SerializedObject(PrefabUtility.GetCorrespondingObjectFromSource(prop.serializedObject.targetObject)).FindProperty(prop.name);
				if (PropEquals(prop, preProp))
				{
					prop.prefabOverride = false;
				}

			}
		}

	}


	bool PropEquals(SerializedProperty a, SerializedProperty b)
	{
		if (a == null || b == null) return false;
		if (!a.type.Equals(b.type)) return false;
		switch (a.propertyType)
		{
			case SerializedPropertyType.AnimationCurve:
				return a.animationCurveValue.Equals(b.animationCurveValue);
			case SerializedPropertyType.ArraySize:
				return a.arraySize == b.arraySize;
			case SerializedPropertyType.Boolean:
				return a.boolValue == b.boolValue;
			case SerializedPropertyType.Bounds:
				return a.boundsValue == b.boundsValue;
			case SerializedPropertyType.Character:
				return a.stringValue.Equals(b.stringValue);
			case SerializedPropertyType.Color:
				return a.colorValue.Equals(b.colorValue);
			case SerializedPropertyType.Enum:
				return a.enumValueIndex == b.enumValueIndex;
			case SerializedPropertyType.Float:
				return a.floatValue.Equals(b.floatValue);
			case SerializedPropertyType.Generic:
				return false;
			case SerializedPropertyType.Gradient:
				return false;
			case SerializedPropertyType.Integer:
				return a.intValue == b.intValue;
			case SerializedPropertyType.LayerMask:
				return a.intValue == b.intValue;
			case SerializedPropertyType.ObjectReference:
				return a.objectReferenceValue.GetInstanceID() == b.objectReferenceValue.GetInstanceID();
			case SerializedPropertyType.Quaternion:
				return a.quaternionValue.Equals(b.quaternionValue);
			case SerializedPropertyType.Rect:
				return a.rectValue.Equals(b.rectValue);
			case SerializedPropertyType.String:
				return a.stringValue.Equals(b.stringValue);
			case SerializedPropertyType.Vector2:
				return a.vector2Value.Equals(b.vector2Value);
			case SerializedPropertyType.Vector3:
				return a.vector3Value.Equals(b.vector3Value);
			case SerializedPropertyType.Vector4:
				return a.vector4Value.Equals(b.vector4Value);
			default:
				return false;
		}
	}

	private void DrawArrayProperty(SerializedProperty prop)
	{
		EditorGUILayout.PropertyField(prop);
		if (prop.isExpanded)
		{
			EditorGUI.indentLevel++;
			SerializedProperty propChild = prop.Copy();
			propChild.NextVisible(true);
			EditorGUILayout.PropertyField(propChild);
			bool arrayDiff = true;
			SerializedProperty preProp = null;
			if (prop.isInstantiatedPrefab)
				preProp = new SerializedObject(PrefabUtility.GetCorrespondingObjectFromSource(prop.serializedObject.targetObject)).FindProperty(prop.name);
			for (int i = 0; i < prop.arraySize; i++)
			{
				SerializedProperty item = prop.GetArrayElementAtIndex(i);
				if (item.isArray)
				{
					DrawArrayProperty(item);
				}
				else
				{
					EditorGUILayout.PropertyField(item);
					if (prop.isInstantiatedPrefab)
					{
						if (preProp != null && preProp.arraySize == prop.arraySize && PropEquals(item, preProp.GetArrayElementAtIndex(i)))
						{
							item.prefabOverride = false;

						}
						else
						{
							arrayDiff = false;
						}

					}
				}

			}
			if (arrayDiff)
			{
				prop.prefabOverride = false;
			}
			EditorGUI.indentLevel--;
		}

	}

	void OnFocus()
	{
		RefreshContent();
	}
	void OnLostFocus()
	{
		RefreshContent();
	}
	void OnHeirarchyChange()
	{
		RefreshContent();
	}



}