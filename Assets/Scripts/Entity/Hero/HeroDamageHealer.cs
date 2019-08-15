using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDamageHealer : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new HealingDamageActionProvider(this, damageType);
    }

    protected override string GetAdditionalDescription()
    {
        return "Ranged healer, damages enemies to heal self, spends energy to heal all allies";
    }

    protected override string GetClassType() => "Healer";

    protected override int GetStartHealth() => HeroHealingData.Instance.StartHealth + startHealthAdd;
}
