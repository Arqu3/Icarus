using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDebuffSupport : HeroSupport
{
    protected override IActionProvider CreateActionProvider()
    {
        return new DebuffSupportActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged support, debuffs enemy output, spends energy to debuff enemy cooldown times";
    }
}
