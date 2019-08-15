using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRanger : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new RangedActionProvider(this, damageType, projectilePrefab);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged damage dealer, spends energy to shoot multiple projectiles in a cone";
    }

    protected override string GetClassType() => "Ranged";

    protected override int GetStartHealth() => HeroRangedData.Instance.StartHealth + startHealthAdd;
}
