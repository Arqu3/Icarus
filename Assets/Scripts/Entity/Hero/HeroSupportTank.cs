using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSupportTank : HeroTank
{
    const float OverrideBlockchance = 0.15f;

    protected override IActionProvider CreateActionProvider()
    {
        return new SupportTankActionProvider(this, damageType);
    }

    protected override IEntityHealthProvider CreateHealthProvider(int startHealth)
    {
        var health = base.CreateHealthProvider(startHealth);
        return new HealthBlockDecorator(health as BaseEntityHealthProvider, blockChance = OverrideBlockchance);
    }

    protected override string GetAdditionalDescription()
    {
        blockChance = OverrideBlockchance;
        return base.GetAdditionalDescription() + ", buffs ally energy gain";
    }
}
