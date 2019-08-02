using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActionProvider : MeleeActionProvider
{
    public EnemyMeleeActionProvider(ICombatEntity owner, float range, DamageType damageType) : base(owner, range, damageType)
    {
    }

    protected override float ActionCooldown => 1f;
}
