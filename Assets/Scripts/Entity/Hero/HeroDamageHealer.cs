using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDamageHealer : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new HealingDamageActionProvider(this, damageType);
    }
}
