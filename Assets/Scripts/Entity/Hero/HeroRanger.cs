using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRanger : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new RangedActionProvider(this, damageType, projectilePrefab);
    }
}
