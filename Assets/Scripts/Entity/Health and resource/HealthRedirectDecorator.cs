using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRedirectDecorator : HealthDecorator
{
    ICombatEntity redirect;
    float redirectPercentage;

    public HealthRedirectDecorator(BaseEntityHealthProvider provider, ICombatEntity redirect, float redirectPercentage) : base(provider)
    {
        this.redirect = redirect;
        this.redirectPercentage = redirectPercentage;
    }

    public override DamageResult Remove(int amount, DamageType type)
    {
        int i = Mathf.CeilToInt(amount * redirectPercentage);
        redirect.RemoveHealth(i, type);
        return base.Remove(amount - i, type);
    }

    public override DamageResult RemovePercentage(float percentage, DamageType type)
    {
        redirect.RemoveHealthPercentage(percentage * redirectPercentage, type);
        return base.RemovePercentage(percentage * (1f - redirectPercentage), type);
    }
}
