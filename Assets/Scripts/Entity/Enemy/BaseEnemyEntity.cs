using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyEntity : BaseEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        switch (attackType)
        {
            case AttackType.Melee:
                return new EnemyMeleeActionProvider(this, damageType);
            case AttackType.Ranged:
                return new EnemyRangedActionProvider(this, damageType, projectilePrefab);
            default:
                break;
        }
        Debug.LogError("No action provider created!");
        return null;
    }

    #region Combat entity interface

    public override float Resource => 0f;

    public override float ResourcePercentage => 0f;

    public override EntityType EntityType => EntityType.Enemy;

    public override void GiveResource(float amount){}

    public override void GiveResourcePercentage(float percentage) { }

    public override bool SpendResource(float amount)
    {
        return false;
    }

    public override bool SpendResourcePercentage(float percentage)
    {
        return false;
    }

    protected override IEntityResourceProvider CreateResourceProvider()
    {
        return null;
    }

    #endregion
}
