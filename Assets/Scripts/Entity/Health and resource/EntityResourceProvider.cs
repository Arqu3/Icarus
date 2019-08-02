using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityResourceProvider : IEntityResourceProvider
{
    public EntityResourceProvider(float startAmount)
    {
        Max = startAmount;
        Current = 0f;
    }

    public float Current { get; private set; }
    public float Percentage => Current / Max;
    public float Max { get; private set; }

    public bool Spend(float amount)
    {
        float absClamped = Mathf.Clamp(Mathf.Abs(amount), 0f, Max);

        if (absClamped < Current)
        {
            Current -= absClamped;
            return true;
        }

        return false;
    }

    public bool SpendPercentage(float percentage)
    {
        return Spend(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f));
    }

    public void Give(float amount)
    {
        Current = Mathf.Clamp(Current + Mathf.Abs(amount), 0f, Max);
    }

    public void Update()
    {
        Current = Mathf.Clamp(Current + Time.deltaTime, 0f, Max);
    }
}
