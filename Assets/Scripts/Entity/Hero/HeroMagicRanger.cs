using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMagicRanger : HeroRanger
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MagicRangedActionProvider(this, damageType, projectilePrefab);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged damage dealer, shoots lightning bolts that bounce between enemies, spends energy to shoot 3 lightning bolts at once";
    }
}