using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRogue : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MeleeRogueActionProvider(this, damageType);
    }
}
