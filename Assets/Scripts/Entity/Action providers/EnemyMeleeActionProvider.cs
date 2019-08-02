using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActionProvider : MeleeActionProvider
{
    public EnemyMeleeActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    protected override BaseStatProvider CreateStatProvider()
    {
        return new DefaultStatProvider(EnemyMeleeData.Instance.Power, EnemyMeleeData.Instance.ResourceGain, EnemyMeleeData.Instance.ActionCooldown, EnemyMeleeData.Instance.Range);
    }
}
