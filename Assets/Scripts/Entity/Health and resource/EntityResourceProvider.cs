using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityResourceProvider : BaseEntityResourceProvider
{
    float Max, Current;

    public EntityResourceProvider(float startAmount)
    {
        Max = startAmount;
        Current = 0f;
    }

    public override bool Spend(float amount)
    {
        float absClamped = Mathf.Clamp(Mathf.Abs(amount), 0f, Max);

        if (absClamped < Current)
        {
            Current -= absClamped;
            return true;
        }

        return false;
    }

    public override bool SpendPercentage(float percentage)
    {
        return Spend(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f));
    }

    public override void Give(float amount)
    {
        Current = Mathf.Clamp(Current + Mathf.Abs(amount), 0f, Max);
    }

    public override void GivePercentage(float percentage)
    {
        Give(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f));
    }

    public override void Update()
    {
        Current = Mathf.Clamp(Current + Time.deltaTime, 0f, Max);
    }

    public override float GetCurrent()
    {
        return Current;
    }

    public override float GetMax()
    {
        return Max;
    }

    public override float GetPercentage()
    {
        return GetCurrent() / GetMax();
    }
}
