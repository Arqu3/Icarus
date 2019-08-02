using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu()]
public class CamerashakeProfileData : ScriptableObject
{
    public static readonly string ResourcePath = "CameraShakeData";
    public static readonly string ResourceName = "ProfileData";

    public static readonly string FirstAssembly = "Assembly-CSharp-firstpass";
    public static readonly string SecondAssembly = "Assembly-CSharp";

    //private static CamerashakeProfileData _Instance;

    public static CamerashakeProfileData Instance
    {
        get
        {
            return Resources.Load<CamerashakeProfileData>(ResourcePath + "/" + ResourceName);//_Instance ?? (_Instance = Resources.Load<CamerashakeProfileData>(ResourcePath + "/" + ResourceName));
        }
    }

    [SerializeField]
    List<CameraShakeProfile> profiles = new List<CameraShakeProfile>();

    public CameraShakeProfile Get(int index)
    {
        return profiles[index];
    }

    public CameraShakeProfile Get(string name)
    {
        var pro = (from p in profiles where p.GetName() == name select p).FirstOrDefault();
        if (pro == default(CameraShakeProfile)) Debug.LogWarning("Could not find profile with name " + name);

        return pro;
    }

    public void AddProfile(CameraShakeProfile profile)
    {
        if (profiles.AddIfNotPresent(profile))
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }

    public CameraShakeLayer[] GetPrefabs()
    {
        return Resources.LoadAll<CameraShakeLayer>(ResourcePath + "/Prefabs");
    }

    public List<CameraShakeProfile> AllProfiles
    {
        get
        {
            return profiles;
        }
    }

#if UNITY_EDITOR

    public void EDITORONLY_RefreshPrefabs()
    {
        string relativePath = "Resources/" + ResourcePath + "/Prefabs";
        string prefabPath = Application.dataPath + "/" + relativePath;
        if (!Directory.Exists(prefabPath))
        {
            Directory.CreateDirectory(prefabPath);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        RefreshPrefabs(relativePath, Assembly.Load(FirstAssembly));
        RefreshPrefabs(relativePath, Assembly.Load(SecondAssembly));
    }

    private void RefreshPrefabs(string relativeAssetPath, Assembly assembly)
    {
        var componentList = GetBehaviourTypesInAssembly(typeof(CameraShakeLayer), assembly);

        AssetDatabase.StartAssetEditing();

        for (int i = 0; i < componentList.Length; ++i)
        {
            var gObj = new GameObject(componentList[i].Name);
            gObj.AddComponent(componentList[i]);

            PrefabUtility.SaveAsPrefabAsset(gObj, "Assets/" + relativeAssetPath + "/" + gObj.name + ".prefab");

            DestroyImmediate(gObj);
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static System.Type[] GetBehaviourTypesInAssembly(System.Type baseType, Assembly assembly)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType).ToArray();
    }

#endif
}

public static class ListExtensions
{
    /// <summary>
    /// Adds an item to this list if it does not contain it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <returns>True if list added item</returns>
    public static bool AddIfNotPresent<T>(this List<T> list, T item)
    {
        bool contains = list.Contains(item);
        if (!contains) list.Add(item);
        return !contains;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CamerashakeProfileData))]
public class CamerashakeProfileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        CamerashakeProfileData inspectorData = target as CamerashakeProfileData;

        if (inspectorData)
        {
            if (GUILayout.Button("Refresh prefabs"))
            {
                inspectorData.EDITORONLY_RefreshPrefabs();
            }
        }
    }
}

public static class ScriptableObjectExtension
{
    /// <summary>
    /// Creates a new scriptableobject asset at the path relative to application.datapath
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativePath"></param>
    /// <param name="assetname"></param>
    /// <returns></returns>
    public static T CreateInstanceAndAsset<T>(string relativePath, string assetname) where T : ScriptableObject
    {
        string directoryPath = Application.dataPath + "/" + relativePath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.Log("Could not find directory at path " + directoryPath + ", created it");
        }

        assetname += assetname.EndsWith(".asset") ? "" : ".asset";
        if (!File.Exists(directoryPath + "/" + assetname))
        {
            var instance = ScriptableObject.CreateInstance<T>();

            string assetPathAndName = relativePath + "/" + assetname;

            if (!assetPathAndName.StartsWith("Assets/")) assetPathAndName = assetPathAndName.Insert(0, "Assets/");

            AssetDatabase.CreateAsset(instance, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Could not find asset at path " + directoryPath + ", created it");

            return instance;
        }
        else
        {
            Debug.LogWarning("Asset already exists at: " + directoryPath);
            return null;
        }
    }
}

#endif