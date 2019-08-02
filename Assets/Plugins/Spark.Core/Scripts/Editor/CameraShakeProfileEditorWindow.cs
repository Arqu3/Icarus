using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraShakeProfileEditorWindow : EditorWindow
{
    static CamerashakeProfileData cData;
    static string profileName = "";

    public static CameraShakeProfileEditorWindow CreateWindow(CamerashakeProfileData contextData)
    {
        if (contextData == null)
        {
            Debug.LogWarning("Contextdata is null!");
            return null;
        }

        cData = contextData;
        var window = GetWindow(typeof(CameraShakeProfileEditorWindow)) as CameraShakeProfileEditorWindow;

        Vector2 referenceSize = new Vector2(300f, 100f);
        window.minSize = window.maxSize = referenceSize;

        profileName = "";

        return window;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Profile name");
        profileName = GUILayout.TextField(profileName);

        EditorGUILayout.Space();

        if (!string.IsNullOrEmpty(profileName) && cData != null)
        {
            if (GUILayout.Button("Create new profile"))
            {
                cData.AddProfile(CameraShakeProfile.EDITORONLY_CreateProfile(profileName));
                Close();
            }

            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Close")) Close();
    }
}
