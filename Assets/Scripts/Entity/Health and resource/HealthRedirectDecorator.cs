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

    public override DamageResult Remove(int amount)
    {
        int i = Mathf.CeilToInt(amount * redirectPercentage);
        redirect.RemoveHealth(i);
        return base.Remove(amount - i);
    }

    public override DamageResult RemovePercentage(float percentage)
    {
        redirect.RemoveHealthPercentage(percentage * redirectPercentage);
        return base.RemovePercentage(percentage * (1f - redirectPercentage));
    }
}
