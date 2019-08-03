using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraProjectilesDecorator : RangedStatDecorator
{
    int additionalProjectiles;

    public ExtraProjectilesDecorator(BaseRangedStatProvider provider, int additionalProjectiles) : base(provider)
    {
        this.additionalProjectiles = additionalProjectiles;
    }

    public override int GetProjectileCount()
    {
        return base.GetProjectileCount() + additionalProjectiles;
    }
}
