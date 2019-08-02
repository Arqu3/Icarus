using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringEditor : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		position.height /= 3;
		var guid = property.FindPropertyRelative("id");
		if (string.IsNullOrEmpty(guid.stringValue))
		{
			guid.stringValue = System.Guid.NewGuid().ToString();
		}
		EditorGUI.BeginProperty(position, label, property);
		var val = property.FindPropertyRelative("baseValue");
		EditorGUI.PropertyField(position, val, label);
		position.x += 30;
		position.width -= 30;
		position.y += EditorGUIUtility.singleLineHeight;
		EditorGUI.PropertyField(position, guid);
		var desc = property.FindPropertyRelative("description");
		position.y += EditorGUIUtility.singleLineHeight;
		EditorGUI.PropertyField(position, desc);
		EditorGUI.EndProperty();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight * 3;
	}
}*/
