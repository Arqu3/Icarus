using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeActionProvider : BaseActionProvider
{
    DamageType damageType;
    NavMeshAgent agent;

    float hitTimestamp;
    const float BaseAttackCooldown = 0.5f;

    public MeleeActionProvider(ICombatEntity owner, float range, DamageType damageType) : base(owner, range)
    {
        agent = owner.gameObject.GetComponent<NavMeshAgent>();
        this.damageType = damageType;
    }

    public override void Update()
    {
        if (!HasTarget) Target = LookForRandomEnemyTarget();
        else
        {
            UpdateMovement();

            if (IsInRange)
            {
                if (owner.SpendResourcePercentage(0.99f)) PerformSpecial();
                else PerformBasic();
            }
        }
    }

    protected override void PerformBasic()
    {
        if (Time.time - hitTimestamp < BaseAttackCooldown) return;

        Target.RemoveHealth(10);
        owner.GiveResource(0.34f);

        hitTimestamp = Time.time;
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealth(10);
        var hits = Physics.OverlapSphere(owner.transform.position, 5f);

        foreach(var hit in hits)
        {
            var entity = hit.GetComponent<BaseEntity>();
            if (entity && entity.EntityType != owner.EntityType) entity.RemoveHealth(20);
        }
    }
}
