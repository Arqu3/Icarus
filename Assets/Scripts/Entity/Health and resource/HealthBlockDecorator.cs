using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBlockDecorator : HealthDecorator
{
    float blockChance;

    public HealthBlockDecorator(BaseEntityHealthProvider provider, float blockChance) : base(provider)
    {
        this.blockChance = blockChance;
    }

    public override DamageResult Remove(int amount, DamageType type)
    {
        if (Random.Range(0f, 1f) <= blockChance) return DamageResult.Blocked;

        return base.Remove(amount, type);
    }
}
