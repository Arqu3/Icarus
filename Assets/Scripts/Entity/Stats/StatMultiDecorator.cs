using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMultiDecorator : BaseStatDecorator
{
    float CDMulti, resourceMulti, rangeMulti, powerMulti;

    public StatMultiDecorator(BaseStatProvider provider, float powerMulti, float CDMulti, float resourceMulti, float rangeMulti) : base(provider)
    {
        this.powerMulti = powerMulti;
        this.CDMulti = CDMulti;
        this.resourceMulti = resourceMulti;
        this.rangeMulti = rangeMulti;
    }

    public override float GetActionCooldown()
    {
        return base.GetActionCooldown() * CDMulti;
    }

    public override int GetPower()
    {
        return Mathf.CeilToInt(base.GetPower() * powerMulti);
    }

    public override float GetRange()
    {
        return base.GetRange() * rangeMulti;
    }

    public override float GetResourceGain()
    {
        return base.GetResourceGain() * resourceMulti;
    }
}
