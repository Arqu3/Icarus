using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeRotationLayer : CameraShakeLayer
{
    private void Start()
    {
        onLoop.AddListener((x, y, z) =>
        {
            transform.localRotation = localStartRot * Quaternion.Euler(x, y, z);
        });
    }
}
