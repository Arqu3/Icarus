using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(Util.ObservableValueBase), true)]
public class ObservableDrawer : PropertyDrawer {
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		EditorGUI.BeginProperty( position, label, property );
		var prop = property.FindPropertyRelative( "_value" );
		if ( Application.isPlaying )
		{
			var o = property.serializedObject.targetObject;
			var field = o.GetType().GetField( property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			var v = field.GetValue( o ) as Util.ObservableValueBase;
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField( position, prop, label );
			if (EditorGUI.EndChangeCheck())
				v.AlertAll();
		}
		else
		{
			EditorGUI.PropertyField( position, prop, label );
		}
		EditorGUI.EndProperty();
	}
}
