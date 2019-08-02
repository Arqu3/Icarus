using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(CameraShakeMaster))]
public class CameraShakeMasterInspector : Editor
{
    int selectedIndex = 0;
    List<System.Type> layerTypes = new List<System.Type>();
    CamerashakeProfileData data;

    private void OnEnable()
    {
        data = CamerashakeProfileData.Instance;

        var fromA1 = CamerashakeProfileData.GetBehaviourTypesInAssembly(typeof(CameraShakeLayer), Assembly.Load(CamerashakeProfileData.FirstAssembly));
        var fromA2 = CamerashakeProfileData.GetBehaviourTypesInAssembly(typeof(CameraShakeLayer), Assembly.Load(CamerashakeProfileData.SecondAssembly));

        foreach (var t in fromA1) layerTypes.Add(t);
        foreach (var t in fromA2) layerTypes.Add(t);
    }

    private void OnDisable()
    {
        if (popupWindow)
        {
            popupWindow.Close();
            DestroyImmediate(popupWindow);
        }
    }

    CameraShakeProfileEditorWindow popupWindow;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (data)
        {
            EditorGUILayout.LabelField("PROFILE DATA", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Data found at: " + "Resources/" + CamerashakeProfileData.ResourcePath, CamerashakeProfileData.ResourceName);
            EditorGUILayout.Space();

            CameraShakeMaster targetMaster = target as CameraShakeMaster;
            if (targetMaster)
            {
                EditorGUILayout.LabelField("Profile data to this master");
                if (GUILayout.Button("Assign data to master"))
                {
                    targetMaster.EDITORONLY_SetData(data);
                    EditorSceneManager.MarkAllScenesDirty();
                }

                EditorGUILayout.Space();

                if (targetMaster.HasData())
                {
                    EditorGUILayout.LabelField("ADD PROFILE", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Create new profile");
                    if (GUILayout.Button("Add new profile to data"))
                    {
                        popupWindow = CameraShakeProfileEditorWindow.CreateWindow(data);
                    }
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("No profile data found");
            if (GUILayout.Button("Create profile data"))
            {
                var data = ScriptableObjectExtension.CreateInstanceAndAsset<CamerashakeProfileData>("Resources/" + CamerashakeProfileData.ResourcePath, CamerashakeProfileData.ResourceName);
                data?.EDITORONLY_RefreshPrefabs();
                this.data = CamerashakeProfileData.Instance;
                (target as CameraShakeMaster).EDITORONLY_SetData(this.data);
            }
        }

        if (data && data.AllProfiles.Count > 0)
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("PROFILE EDITING", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Select profile to edit");

            string[] options = data.AllProfiles.Select(x => x.GetName()).ToArray();

            selectedIndex = EditorGUILayout.Popup("Profiles", selectedIndex, options);

            var selectedProfile = data.AllProfiles[selectedIndex];

            if (selectedProfile.AllLayers.Count > 0)
            {
                EditorGUILayout.LabelField("Contains:");
                CameraShakeLayer markedForDelete = null;
                GUI.backgroundColor = Color.red;
                foreach (var l in selectedProfile.AllLayers)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(l.name);
                    if (GUILayout.Button("Remove"))
                    {
                        markedForDelete = l;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (markedForDelete != null) selectedProfile.EDITORONLY_RemoveLayer(markedForDelete);
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("ADD LAYERS TO SELECTED PROFILE", EditorStyles.boldLabel);

            GUI.backgroundColor = Color.green;

            foreach(var t in layerTypes)
            {
                if (GUILayout.Button("Add " + t.Name))
                {
                    selectedProfile.EDITORONLY_AddLayer(t);
                }
            }

            GUI.backgroundColor = Color.white;

            if (selectedProfile.AllLayers.Count > 0)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("INSTANTIATE PROFILE LAYERS", EditorStyles.boldLabel);

                if (GUILayout.Button("Instantiate by adding"))
                {
                    InstantiateProfile(selectedProfile, (target as CameraShakeMaster).gameObject, InstantiationMode.Add);
                }
                else if (GUILayout.Button("Instantiate by adding missing types"))
                {
                    InstantiateProfile(selectedProfile, (target as CameraShakeMaster).gameObject, InstantiationMode.AddMissing);
                }
                else if (GUILayout.Button("Instantiate by replacing"))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "This will remove any layers currently present in the hierarchy", "Confirm", "Cancel"))
                    InstantiateProfile(selectedProfile, (target as CameraShakeMaster).gameObject, InstantiationMode.Replace);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("DESTROY INSTANTIATED LAYERS", EditorStyles.boldLabel);

            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Destroy parent layers"))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "This will remove any layers current present in the hierarchy", "Confirm", "Cancel"))
                {
                    var targetmaster = target as CameraShakeMaster;
                    var layers = targetmaster.GetComponentsInParent<CameraShakeLayer>();
                    RemoveParentedLayers(layers, targetmaster.gameObject);
                }
            }
        }
    }

    enum InstantiationMode
    {
        Add,
        AddMissing,
        Replace
    }

    void InstantiateProfile(CameraShakeProfile profile, GameObject target, InstantiationMode mode)
    {
        var layers = target.GetComponentsInParent<CameraShakeLayer>();
        switch (mode)
        {
            case InstantiationMode.Add:

                for (int i = 0; i < profile.AllLayers.Count; ++i)
                {
                    AddLayer(profile.AllLayers[i], target);
                }

                break;
            case InstantiationMode.AddMissing:

                for (int i = 0; i < profile.AllLayers.Count; ++i)
                {
                    System.Type type = profile.AllLayers[i].GetType();
                    if ((from t in layers where t.GetType() == type select t).Count() == 0) AddLayer(profile.AllLayers[i], target);
                }

                break;
            case InstantiationMode.Replace:

                RemoveParentedLayers(layers, target);

                for (int i = 0; i < profile.AllLayers.Count; ++i)
                {
                    AddLayer(profile.AllLayers[i], target);
                }

                break;
            default:
                break;
        }

        EditorSceneManager.MarkAllScenesDirty();
    }

    void RemoveParentedLayers(CameraShakeLayer[] layers, GameObject target)
    {
        target.transform.SetParent(null);
        List<GameObject> leftOvers = new List<GameObject>();
        foreach (var l in layers)
        {
            var components = (from lt in l.GetComponents<Component>() where lt.GetType() != typeof(Transform) select lt).ToArray();

            if ((from c in components where !typeof(CameraShakeLayer).IsAssignableFrom(c.GetType()) select c).Count() > 0)
            {
                Debug.LogWarning("Found other components on " + l.gameObject.name);
                leftOvers.Add(l.gameObject);
                DestroyImmediate(l);
            }
            else DestroyImmediate(l.gameObject);
        }

        if (leftOvers.Count > 0) target.transform.SetParent(leftOvers[0].transform);
    }

    void AddLayer(CameraShakeLayer layer, GameObject target)
    {
        var l = Instantiate(layer);
        l.transform.position = target.transform.position;
        l.transform.rotation = target.transform.rotation;
        target.transform.root.SetParent(l.transform);
    }
}