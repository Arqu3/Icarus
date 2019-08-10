using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWarrior : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new MeleeActionProvider(this, damageType);
    }
}
