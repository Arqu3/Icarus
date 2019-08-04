using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedStatProvider : BaseStatProvider
{
    int Power, ProjectileCount;
    float ResourceGain, ActionCooldown, Range;

    public RangedStatProvider(int Power, float ResourceGain, float ActionCooldown, int ProjectileCount, float Range)
    {
        this.Power = Power;
        this.ResourceGain = ResourceGain;
        this.ActionCooldown = ActionCooldown;
        this.ProjectileCount = ProjectileCount;
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

    public override int GetProjectileCount()
    {
        return ProjectileCount;
    }

    public override float GetResourceGain()
    {
        return ResourceGain;
    }

    public override float GetRange()
    {
        return Range;
    }
}
