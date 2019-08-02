using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeFOVLayer : CameraShakeLayer
{
    [Header("Camera field of view")]
    [SerializeField]
    float fieldOfViewMulti = 0.05f;

    float startFOV;

    private void Start()
    {
        useY = false;
        useZ = false;

        Camera cam = Camera.main;

        if (!cam) cam = GetComponentInChildren<Camera>();

        startFOV = cam.fieldOfView;

        onLoop.AddListener((x, y, z) =>
        {
            cam.fieldOfView = startFOV + (startFOV * fieldOfViewMulti * x);
        });

        onReset.AddListener(() =>
        {
            cam.fieldOfView = startFOV;
        });
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        useY = false;
        useZ = false;
    }

#endif
}
