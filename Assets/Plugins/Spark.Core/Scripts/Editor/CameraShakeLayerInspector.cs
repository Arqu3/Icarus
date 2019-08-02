using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeLayer), true)]
public class CameraShakeLayerInspector : Editor
{
    CameraShakeMaster master;
    CameraShakeLayer targetLayer;

    private void OnEnable()
    {
        targetLayer = target as CameraShakeLayer;
        master = targetLayer.GetComponentInChildren<CameraShakeMaster>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Duration (based on longest curve): " + targetLayer.Duration);

        if (!master) return;

        EditorGUILayout.Space();

        if (GUILayout.Button("Select master")) Selection.activeGameObject = master.gameObject;
    }
}
