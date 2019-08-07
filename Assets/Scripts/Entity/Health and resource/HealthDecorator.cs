using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDecorator : BaseEntityHealthProvider, IHealthDecorator
{
    public BaseEntityHealthProvider provider { get; set; }

    public HealthDecorator(BaseEntityHealthProvider provider)
    {
        this.provider = provider;
    }

    public override int GetCurrent()
    {
        return provider.GetCurrent();
    }

    public override int GetMax()
    {
        return provider.GetMax();
    }

    public override float GetPercentage()
    {
        return (float)GetCurrent() / GetMax();
    }

    public override void Give(int amount)
    {
        provider.Give(amount);
    }

    public override void GivePercentage(float percentage)
    {
        provider.GivePercentage(percentage);
    }

    public override DamageResult Remove(int amount)
    {
        return provider.Remove(amount);
    }

    public override DamageResult RemovePercentage(float percentage)
    {
        return provider.RemovePercentage(percentage);
    }
}
