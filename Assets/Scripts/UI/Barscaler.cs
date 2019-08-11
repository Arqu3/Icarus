using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barscaler : MonoBehaviour
{
    [Header("Bar to scale")]
    [SerializeField]
    Image bar;

    public void UpdateScale(float percentage)
    {
        bar.transform.localScale = new Vector3(Mathf.MoveTowards(bar.transform.localScale.x, percentage, 2f * Time.deltaTime), 1f, 1f);
    }
}
