using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMagicRanger : HeroRanger
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MagicRangedActionProvider(this, damageType, projectilePrefab);
    }
}