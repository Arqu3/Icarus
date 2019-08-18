using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTank : HeroEntity
{
    protected float blockChance = 0.2f;

    protected override IActionProvider CreateActionProvider()
    {

        return new TankActionProvider(this, damageType);
    }

    protected override IEntityHealthProvider CreateHealthProvider(int startHealth)
    {
        var health = base.CreateHealthProvider(startHealth);
        return new HealthBlockDecorator(health as BaseEntityHealthProvider, blockChance);
    }

    protected override string GetAdditionalDescription()
    {
        return "Melee tank, spends energy to taunt enemies, has a " + (blockChance * 100f).ToString(".##") + "% chance to block incoming damage";
    }

    protected override string GetClassType() => "Tank";

    protected override int GetStartHealth() => HeroTankData.Instance.StartHealth + startHealthAdd;
}
