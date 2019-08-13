using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPlacement : MonoBehaviour
{
    void Start()
    {
        foreach(var h in HeroCollection.Instance.GetSelected())
        {
            Vector3 r = Random.insideUnitSphere * 4f;
            r.y = 0f;
            HeroCreator.CreateLiveFrom(h, h.Prefab.transform.position + r);
        }
    }
}
