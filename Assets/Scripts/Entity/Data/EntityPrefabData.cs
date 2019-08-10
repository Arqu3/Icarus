using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPrefabData<TData> : ScriptableObject where TData : EntityPrefabData<TData>
{
    static string ResourcePath
    {
        get
        {
            return "Entity Prefabs/" + typeof(TData).Name;
        }
    }
    private static TData _instance;
    public static TData Instance
    {
        get
        {
            return _instance ?? (_instance = Resources.Load<TData>(ResourcePath));
        }
    }
}