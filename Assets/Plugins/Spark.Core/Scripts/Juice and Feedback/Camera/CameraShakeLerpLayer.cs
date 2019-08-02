using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeLerpLayer : CameraShakeLayer
{
    private void Start()
    {
        onLoop.AddListener((x, y, z) =>
        {
            transform.localPosition = localStartpos + new Vector3(x, y, z);
        });
    }
}
