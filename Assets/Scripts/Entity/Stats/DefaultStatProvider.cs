using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStatProvider : BaseStatProvider
{
    int Power;
    float ResourceGain, ActionCooldown, Range;

    public DefaultStatProvider(int Power, float ResourceGain, float ActionCooldown, float Range)
    {
        this.Power = Power;
        this.ResourceGain = ResourceGain;
        this.ActionCooldown = ActionCooldown;
        this.Range = Range;
    }

    public override float GetActionCooldown()
    {
        return ActionCooldown;
    }

    public override int GetPower()
    {
        return Power;
    }

    public override float GetResourceGain()
    {
        return ResourceGain;
    }

    public override float GetRange()
    {
        return Range;
    }

    public override int GetProjectileCount()
    {
        return 1;
    }
}
