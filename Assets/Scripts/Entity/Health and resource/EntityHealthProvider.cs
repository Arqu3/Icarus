using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthProvider : BaseEntityHealthProvider
{
    public int Current, Max;
    public System.Func<IEntityHealthProvider> GetCurrentProvider;

    public EntityHealthProvider(int startAmount)
    {
        Current = Max = startAmount;
    }

    public override DamageResult Remove(int amount)
    {
        int absClamped = Mathf.Clamp(Mathf.Abs(amount), 0, GetCurrentProvider().GetMax());
        Current = Mathf.Clamp(Current - absClamped, 0, GetCurrentProvider().GetMax());
        return DamageResult.Hit;
    }

    public override DamageResult RemovePercentage(float percentage)
    {
        return Remove((int)(GetCurrentProvider().GetMax() * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f)));
    }

    public override void Give(int amount)
    {
        amount = Mathf.Abs(amount);
        Current += amount;
        Current = Mathf.Clamp(Current, 0, GetCurrentProvider().GetMax());
    }

    public override void GivePercentage(float percentage)
    {
        Give((int)(GetCurrentProvider().GetMax() * Mathf.Clamp(Mathf.Abs(percentage), 0f, 1f)));
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

    public override int GetRegenAmount()
    {
        return 0;
    }

    public override float GetRegenInterval()
    {
        return 1f;
    }

    //public void Update()
    //{
    //    Current = Mathf.Clamp(Current + Time.deltaTime, 0f, Max);
    //}
}
