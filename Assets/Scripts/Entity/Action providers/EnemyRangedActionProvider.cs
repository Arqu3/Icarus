using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedActionProvider : RangedActionProvider
{
    public EnemyRangedActionProvider(ICombatEntity owner, DamageType damageType, GameObject projectilePrefab) : base(owner, damageType, projectilePrefab)
    {
    }

    protected override BaseStatProvider CreateStatProvider()
    {
        return new RangedStatProvider(EnemyRangedData.Instance.Power, EnemyRangedData.Instance.ResourceGain, EnemyRangedData.Instance.ActionCooldown, 1, EnemyRangedData.Instance.Range);
    }
}
