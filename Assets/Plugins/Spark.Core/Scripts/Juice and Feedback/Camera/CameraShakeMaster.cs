using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class CameraShakeMaster : MonoBehaviour
{
    #region Public variables

    [SerializeField]
    CamerashakeProfileData data;

    #endregion

    private void Awake()
    {
    }

    public bool HasData()
    {
        return data;
    }

#if UNITY_EDITOR

    /// <summary>
    /// Overrides this master's data, only use this in editor mode
    /// </summary>
    /// <param name="data"></param>
    public void EDITORONLY_SetData(CamerashakeProfileData data)
    {
        this.data = data;
    }

#endif
}