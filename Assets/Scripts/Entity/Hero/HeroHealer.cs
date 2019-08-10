using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealer : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new HealingActionProvider(this, damageType);
    }
}
