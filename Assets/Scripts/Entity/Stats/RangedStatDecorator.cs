using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedStatDecorator : BaseRangedStatProvider, IStatDecorator
{
    public BaseStatProvider provider { get; set; }

    public RangedStatDecorator(BaseRangedStatProvider provider)
    {
        this.provider = provider;
    }

    public override float GetActionCooldown()
    {
        return provider.GetActionCooldown();
    }

    public override int GetPower()
    {
        return provider.GetPower();
    }

    public override int GetProjectileCount()
    {
        return (provider as BaseRangedStatProvider).GetProjectileCount();
    }

    public override float GetResourceGain()
    {
        return provider.GetResourceGain();
    }

    public override float GetRange()
    {
        return provider.GetRange();
    }
}
