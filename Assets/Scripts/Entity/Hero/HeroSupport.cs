using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSupport : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new SupportActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged support, gives energy, spends energy to give energy to allies in targeted radius";
    }

    protected override string GetClassType() => "Support";

    protected override int GetStartHealth() => HeroSupportData.Instance.StartHealth + startHealthAdd;
}
