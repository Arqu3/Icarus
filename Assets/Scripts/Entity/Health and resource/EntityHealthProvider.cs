using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthProvider : BaseEntityHealthProvider
{
    int Current, Max;

    public EntityHealthProvider(int startAmount)
    {
        Current = Max = startAmount;
    }

    public override DamageResult Remove(int amount)
    {
        int absClamped = Mathf.Clamp(Mathf.Abs(amount), 0, Max);
        Current = Mathf.Clamp(Current - absClamped, 0, Max);
        return DamageResult.Hit;
    }

    public override DamageResult RemovePercentage(float percentage)
    {
        return Remove((int)(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f)));
    }

    public override void Give(int amount)
    {
        Current = Mathf.Clamp(Current + Mathf.Abs(amount), 0, Max);
    }

    public override void GivePercentage(float percentage)
    {
        Give((int)(Max * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f)));
    }

    public override int GetCurrent()
    {
        return Current;
    }

    public override int GetMax()
    {
        return Max;
    }

    public override float GetPercentage()
    {
        return (float)GetCurrent() / GetMax();
    }

    //public void Update()
    //{
    //    Current = Mathf.Clamp(Current + Time.deltaTime, 0f, Max);
    //}
}
