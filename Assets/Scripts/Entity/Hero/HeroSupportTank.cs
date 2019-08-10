using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSupportTank : HeroTank
{
    protected override IActionProvider CreateActionProvider()
    {
        baseHealthProvider = new HealthBlockDecorator(baseHealthProvider as BaseEntityHealthProvider, 0.1f);
        return new SupportTankActionProvider(this);
    }
}
