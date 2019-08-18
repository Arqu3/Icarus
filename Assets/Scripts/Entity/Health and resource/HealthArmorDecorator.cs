using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthArmorDecorator : HealthDecorator
{
    DamageType armor;
    float multi;
    public HealthArmorDecorator(BaseEntityHealthProvider provider, DamageType armor, float multi) : base(provider)
    {
        this.armor = armor;
        this.multi = multi;
    }

    public override DamageResult Remove(int amount, DamageType type)
    {
        if (type == armor) amount = Mathf.CeilToInt(amount * multi);

        return base.Remove(amount, type);
    }

    public override DamageResult RemovePercentage(float percentage, DamageType type)
    {
        if (type == armor) percentage *= multi;

        return base.RemovePercentage(percentage, type);
    }
}
