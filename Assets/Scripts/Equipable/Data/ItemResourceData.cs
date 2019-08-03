using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemResourceData<TData> : ScriptableObject where TData : ItemResourceData<TData>
{
    static string ResourcePath
    {
        get
        {
            return "Item Data/" + typeof(TData).Name;
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
