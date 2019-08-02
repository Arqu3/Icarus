using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndResourceBar : MonoBehaviour
{
    [SerializeField]
    Barscaler healthbar;
    [SerializeField]
    Barscaler resourcebar;

    public void UpdateHealth(float percentage)
    {
        healthbar.UpdateScale(percentage);
    }

    public void UpdateResource(float percentage)
    {
        resourcebar.UpdateScale(percentage);
    }
}
