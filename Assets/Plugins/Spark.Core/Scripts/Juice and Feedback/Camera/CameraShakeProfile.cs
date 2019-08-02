using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CameraShakeProfile
{
    public CameraShakeProfile(string name)
    {
        Name = name;
    }

    [SerializeField]
    string Name = "Profile";

    [SerializeField]
    List<CameraShakeLayer> layers = new List<CameraShakeLayer>();

    public string GetName()
    {
        return Name;
    }

    public void PlayAll()
    {
        foreach (var l in layers) l.Play();
    }

    public void PlayAll(float playbackTime)
    {
        foreach (var l in layers) l.Play(playbackTime);
    }

    public void PlayAllLoop()
    {
        foreach (var l in layers) l.PlayLoop();
    }

    public void StopAllLoop()
    {
        foreach (var l in layers) l.StopLoop();
    }

    public List<CameraShakeLayer> AllLayers
    {
        get
        {
            return layers;
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Overrides the current name, only use this in the editor
    /// </summary>
    /// <param name="newname"></param>
    public void EDITORONLY_OverrideName(string newname)
    {
        Name = newname;
    }

    /// <summary>
    /// Adds a layer to this profile unless it already exists in it
    /// </summary>
    /// <param name="layer"></param>
    public void EDITORONLY_AddLayer(Type t)
    {
        var data = CamerashakeProfileData.Instance;
        if (data)
        {
            var prefab = data.GetPrefabs().Where(p => p.GetType() == t).FirstOrDefault();
            if (prefab == default(CameraShakeLayer))
            {
                Debug.LogWarning("Could not find layer with type " + t.ToString());
                return;
            }

            layers.Add(prefab);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            //if (layers.AddIfNotPresent(prefab))
            //{
            //    AssetDatabase.Refresh();
            //    AssetDatabase.SaveAssets();
            //}
        }
    }

    public void EDITORONLY_RemoveLayer(CameraShakeLayer layer)
    {
        layers.Remove(layer);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Creates a new profile
    /// </summary>
    /// <param name="name"></param>
    /// <param name="initialLayers"></param>
    /// <returns></returns>
    public static CameraShakeProfile EDITORONLY_CreateProfile(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var profile = new CameraShakeProfile(name);
            return profile;
        }
        Debug.LogWarning("Profile name is null or empty");
        return null;
    }

#endif
}