using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealer : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new HealingActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged healer, gives health, spends energy to give health + healing over time";
    }

    protected override string GetClassType() => "Healer";

    protected override int GetStartHealth() => HeroHealingData.Instance.StartHealth + startHealthAdd;
}
