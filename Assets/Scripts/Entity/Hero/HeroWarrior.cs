using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWarrior : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MeleeActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Melee damage dealer, spends energy to deal damage to all nearby enemies";
    }

    protected override string GetClassType() => "Melee";

    protected override int GetStartHealth() => HeroMeleeData.Instance.StartHealth + startHealthAdd;
}
