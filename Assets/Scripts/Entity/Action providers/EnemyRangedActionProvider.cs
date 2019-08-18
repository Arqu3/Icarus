using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedActionProvider : RangedActionProvider
{
    public EnemyRangedActionProvider(ICombatEntity owner, DamageType damageType, Projectile projectilePrefab) : base(owner, damageType, projectilePrefab)
    {
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = EnemyRangedData.Instance;
        startHealth = data.StartHealth;
        return new RangedStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, 1, mod.range + data.Range);
    }
}
