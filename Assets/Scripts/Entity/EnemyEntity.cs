using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : BaseEntity
{
    private void Start()
    {
        currentAction = mainAction = new EnemyMeleeActionProvider(this, 3f, DamageType.Physical);
    }

    #region Combat entity interface

    public override float Resource => 0f;

    public override float ResourcePercentage => 0f;

    public override EntityType EntityType => EntityType.Enemy;

    public override void GiveResource(float amount){}

    public override bool SpendResource(float amount)
    {
        return false;
    }

    public override bool SpendResourcePercentage(float percentage)
    {
        return false;
    }

    #endregion
}
