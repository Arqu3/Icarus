using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActionProvider : MeleeActionProvider
{
    public EnemyMeleeActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    public override BaseStatProvider CreateBaseStatProvider()
    {
        return new DefaultStatProvider(EnemyMeleeData.Instance.Power, EnemyMeleeData.Instance.ResourceGain, EnemyMeleeData.Instance.ActionCooldown, EnemyMeleeData.Instance.Range);
    }
}
