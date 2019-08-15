using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTank : HeroEntity
{
    protected float blockChance = 0.2f;

    protected override IActionProvider CreateActionProvider()
    {
        baseHealthProvider = new HealthBlockDecorator(baseHealthProvider as BaseEntityHealthProvider, blockChance);
        return new TankActionProvider(this);
    }

    protected override string GetAdditionalDescription()
    {
        return "Melee tank, spends energy to taunt enemies, has a " + (blockChance * 100f).ToString(".##") + "% chance to block incoming damage";
    }

    protected override string GetClassType() => "Tank";

    protected override int GetStartHealth() => HeroTankData.Instance.StartHealth + startHealthAdd;
}
