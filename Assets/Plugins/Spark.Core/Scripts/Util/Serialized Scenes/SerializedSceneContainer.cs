using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu()]
#endif
public class SerializedSceneContainer : ScriptableObject
{
    [SerializeField]
    List<SceneField> scenes = new List<SceneField>();

    private static SerializedSceneContainer _Instance;

    public static readonly string ResourcePath = "Scene Data";
    public static readonly string ResourceName = "SerializedSceneContainer";

    public static SerializedSceneContainer Instance
    {
        get
        {
            return _Instance ?? (_Instance = Resources.Load<SerializedSceneContainer>(ResourcePath + "/" + ResourceName));
        }
    }

#if UNITY_EDITOR

    public void EDITORONLY_UpdateInternalNames(SceneNameData[] data)
    {
        AssetDatabase.StartAssetEditing();

        for (int i = 0; i < scenes.Count; ++i)
        {
            var newData = (from d in data where scenes[i].SceneName == d.oldName select d).FirstOrDefault();

            if (newData.Equals(default(SceneNameData))) continue;

            if (scenes[i].SceneName != newData.newName) scenes[i].EDITORONLY_UpdateName(newData.newName);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }

#endif
}
