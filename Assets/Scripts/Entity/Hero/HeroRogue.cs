using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRogue : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MeleeRogueActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Melee damage dealer, spends energy to jump to target and gain 100% chance to evade attacks for a short period of time";
    }

    protected override string GetClassType() => "Melee";

    protected override int GetStartHealth() => HeroMeleeData.Instance.StartHealth + startHealthAdd;
}
