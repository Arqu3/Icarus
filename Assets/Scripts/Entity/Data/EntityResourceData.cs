using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityResourceData<TData> : ScriptableObject where TData : EntityResourceData<TData>
{
    static string ResourcePath
    {
        get
        {
            return "Entity Data/" + typeof(TData).Name;
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

    public int StartHealth = 100;
    public int Power = 10;
    public float ResourceGain = 0.25f;
    public float ActionCooldown = 0.5f;
    public float Range = 3f;
}