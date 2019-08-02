using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeActionProvider : BaseActionProvider
{
    protected override float ActionCooldown => HeroMeleeData.Instance.ActionCooldown;

    protected override int Power => HeroMeleeData.Instance.Power;
    protected override float ResourceGain => HeroMeleeData.Instance.ResourceGain;

    DamageType damageType;
    NavMeshAgent agent;

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

            if (IsInRange && !IsOnCooldown)
            {
                if (owner.SpendResourcePercentage(SpecialResourcePercentageCost)) PerformSpecial();
                else PerformBasic();
            }
        }
    }

    protected override void PerformBasic()
    {
        Target.RemoveHealth(Power);
        owner.GiveResource(ResourceGain);

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealth(Power);
        var hits = Physics.OverlapSphere(owner.transform.position, 5f);

        foreach(var hit in hits)
        {
            var entity = hit.GetComponent<BaseEntity>();
            if (entity && entity.EntityType != owner.EntityType) entity.RemoveHealth(Power * 2);
        }

        StartCooldown();
    }
}
