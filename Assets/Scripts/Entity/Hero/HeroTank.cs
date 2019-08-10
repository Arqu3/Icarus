using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTank : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        baseHealthProvider = new HealthBlockDecorator(baseHealthProvider as BaseEntityHealthProvider, 0.2f);
        return new TankActionProvider(this);
    }
}
