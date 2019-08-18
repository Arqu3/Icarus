using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRangedActionProvider : RangedActionProvider
{
    public MagicRangedActionProvider(ICombatEntity owner, DamageType damageType, Projectile projectilePrefab) : base(owner, damageType, projectilePrefab)
    {
    }

    protected override void PerformSpecial()
    {
        var hits = GetEnemyEntitiesInSphere(Target.transform.position, 5f);

        for(int i = 0; i < Mathf.Min(3, hits.Length); ++i)
        {
            Shoot(hits[i].transform.position + Vector3.up * 10f, Quaternion.LookRotation(Vector3.down), false);
        }

        StartCooldown();
    }
}
