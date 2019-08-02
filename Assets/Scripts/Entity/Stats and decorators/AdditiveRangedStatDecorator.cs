using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveRangedStatDecorator : RangedStatDecorator
{
    int additionalProjectiles;

    public AdditiveRangedStatDecorator(BaseRangedStatProvider provider, int additionalProjectiles) : base(provider)
    {
        this.additionalProjectiles = additionalProjectiles;
    }

    public override int GetProjectileCount()
    {
        return base.GetProjectileCount() + additionalProjectiles;
    }
}
