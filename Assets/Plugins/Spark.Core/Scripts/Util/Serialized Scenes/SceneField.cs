using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneField
{
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName = "";

    public string SceneName
    {
        get { return sceneName; }
    }

    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }

#if UNITY_EDITOR

    public void EDITORONLY_UpdateName(string newName)
    {
        sceneName = newName;
    }

#endif
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);
        var sceneAsset = property.FindPropertyRelative("sceneAsset");
        var sceneName = property.FindPropertyRelative("sceneName");
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        if (sceneAsset != null)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck() || (property != null && property.FindPropertyRelative("sceneName").stringValue !=
                Regex.Match(AssetDatabase.GetAssetOrScenePath(sceneAsset.serializedObject.targetObject), "[^\\/]+$").Value))
            {
                sceneAsset.objectReferenceValue = value;
                if (sceneAsset.objectReferenceValue != null)
                {
                    var scenePath = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                    var assetsIndex = scenePath.IndexOf("Assets", StringComparison.Ordinal) + 7;
                    var extensionIndex = scenePath.LastIndexOf(".unity", StringComparison.Ordinal);
                    scenePath = scenePath.Substring(assetsIndex, extensionIndex - assetsIndex);
                    sceneName.stringValue = Regex.Match(scenePath, "[^\\/]+$").Value;
                }
            }
        }
        EditorGUI.EndProperty();
    }
}
#endif
