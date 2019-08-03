using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAddDecorator : BaseStatDecorator
{
    float extraCD, extraResource, extraRange;
    int extraPower;

    public StatAddDecorator(BaseStatProvider provider, int extraPower, float extraCD, float extraResource, float extraRange) : base(provider)
    {
        this.extraPower = extraPower;
        this.extraCD = extraCD;
        this.extraResource = extraResource;
        this.extraRange = extraRange;
    }

    public override float GetActionCooldown()
    {
        return base.GetActionCooldown() + extraCD;
    }

    public override int GetPower()
    {
        return base.GetPower() + extraPower;
    }

    public override float GetRange()
    {
        return base.GetRange() + extraRange;
    }

    public override float GetResourceGain()
    {
        return base.GetResourceGain() + extraResource;
    }
}
