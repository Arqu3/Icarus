using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDecorator : BaseEntityHealthProvider
{
    protected BaseEntityHealthProvider provider;

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
        return provider.GetPercentage();
    }

    public override void Give(int amount)
    {
        provider.Give(amount);
    }

    public override void GivePercentage(float percentage)
    {
        provider.GivePercentage(percentage);
    }

    public override void Remove(int amount)
    {
        provider.Remove(amount);
    }

    public override void RemovePercentage(float percentage)
    {
        provider.RemovePercentage(percentage);
    }
}
