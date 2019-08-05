using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedActionProvider : RangedActionProvider
{
    public EnemyRangedActionProvider(ICombatEntity owner, DamageType damageType, GameObject projectilePrefab) : base(owner, damageType, projectilePrefab)
    {
    }

    public override BaseStatProvider CreateBaseStatProvider()
    {
        var data = EnemyRangedData.Instance;
        return new RangedStatProvider(data.Power, data.ResourceGain, data.ActionCooldown, 1, data.Range);
    }
}
