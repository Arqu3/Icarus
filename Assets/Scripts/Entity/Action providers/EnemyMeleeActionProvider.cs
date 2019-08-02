using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActionProvider : MeleeActionProvider
{
    public EnemyMeleeActionProvider(ICombatEntity owner, float range, DamageType damageType) : base(owner, range, damageType)
    {
    }

    protected override float ActionCooldown => EnemyMeleeData.Instance.ActionCooldown;
    protected override float ResourceGain => EnemyMeleeData.Instance.ResourceGain;
    protected override int Power => EnemyMeleeData.Instance.Power;
}
