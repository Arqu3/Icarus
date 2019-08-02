using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(Util.StateStackValueBase), true)]
public class StateStackValueDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		var prop = property.FindPropertyRelative("stack");
		EditorGUI.PropertyField(position, prop.GetArrayElementAtIndex(0), label);
		EditorGUI.EndProperty();
	}
}
