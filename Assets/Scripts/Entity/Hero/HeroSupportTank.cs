using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSupportTank : HeroTank
{
    protected override IActionProvider CreateActionProvider()
    {
        blockChance = 0.1f;
        baseHealthProvider = new HealthBlockDecorator(baseHealthProvider as BaseEntityHealthProvider, blockChance);
        return new SupportTankActionProvider(this);
    }

    protected override string GetAdditionalDescription()
    {
        blockChance = 0.1f;
        return base.GetAdditionalDescription() + ", buffs ally energy gain";
    }
}
