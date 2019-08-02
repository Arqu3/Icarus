using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
[CanEditMultipleObjects]
public class BaseEditor : Editor
{

	private Dictionary<string, bool> toggles = new Dictionary<string, bool>();


	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		base.OnInspectorGUI();
		if (!serializedObject.isEditingMultipleObjects)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Internal Values");
			if (DrawObject(target))
			{
				EditorUtility.SetDirty(target);
			}
		}

	}

	private bool DrawObject(object obj)
	{
		var type = obj.GetType();

		var fields = type.GetFields(BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
			.Where(f => f.GetCustomAttributes(typeof(ShowInInspectorAttribute), false).Any()).ToArray();
		var props = type.GetProperties(BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
			.Where(f => f.GetCustomAttributes(typeof(ShowInInspectorAttribute), false).Any()).ToArray();


		if ((fields.Any() || props.Any()))
		{

			EditorGUI.BeginDisabledGroup(true);
			foreach (var prop in props)
			{
				var val = prop.GetValue(obj, null);
				DrawValue(prop.Name, val, prop.PropertyType);
			}
			foreach (var field in fields)
			{
				var val = field.GetValue(obj);
				DrawValue(field.Name, val, field.FieldType);
			}
			EditorGUI.EndDisabledGroup();

			return true;
		}
		return false;
	}

	private void DrawValue(string label, object value, Type type)
	{
		if (typeof(string).IsAssignableFrom(type))
		{
			EditorGUILayout.TextField(label, value as string);
		}
		else if (typeof(IDictionary).IsAssignableFrom(type))
		{
			if (!toggles.ContainsKey(label)) toggles.Add(label, true);
			toggles[label] = EditorGUILayout.Foldout(toggles[label], label);
			if (toggles[label])
			{
				EditorGUI.indentLevel++;
				var en = (value as IDictionary).GetEnumerator();
				while (en.MoveNext())
				{
					EditorGUILayout.TextField(en.Key.ToString(), (en.Value == null ? "null" : en.Value.ToString()));
				}
				EditorGUI.indentLevel--;
			}


		}
		else if (typeof(IEnumerable).IsAssignableFrom(type))
		{
			if (!toggles.ContainsKey(label)) toggles.Add(label, true);
			toggles[label] = EditorGUILayout.Foldout(toggles[label], label);
			if (toggles[label] && value != null)
			{
				EditorGUI.indentLevel++;
				int i = 0;
				foreach (var v in (value as IEnumerable))
				{
					EditorGUILayout.TextField("[" + i + "]", v == null ? "null" : v.ToString());
					i++;
				}
				EditorGUI.indentLevel--;
			}
		}
		else if (type.GetCustomAttributes(typeof(InspectableAttribute), false).Any())
		{
			EditorGUILayout.LabelField(label);
			EditorGUI.indentLevel++;
			DrawObject(value);
			EditorGUI.indentLevel--;
		}
		else
		{
			EditorGUILayout.TextField(label, value == null ? "null" : value.ToString());
		}
	}
}