using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeActionProvider : BaseActionProvider
{
    DamageType damageType;

    public MeleeActionProvider(ICombatEntity owner, DamageType damageType) : base(owner)
    {
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
        Target.RemoveHealth(CurrentStatProvider.GetPower());
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealth(CurrentStatProvider.GetPower());
        var hits = Physics.OverlapSphere(owner.transform.position, 5f);

        foreach(var hit in hits)
        {
            var entity = hit.GetComponent<BaseEntity>();
            if (entity && entity.EntityType != owner.EntityType) entity.RemoveHealth(CurrentStatProvider.GetPower() * 2);
        }

        StartCooldown();
    }

    public override BaseStatProvider CreateBaseStatProvider()
    {
        return new DefaultStatProvider(HeroMeleeData.Instance.Power, HeroMeleeData.Instance.ResourceGain, HeroMeleeData.Instance.ActionCooldown, HeroMeleeData.Instance.Range);
    }
}
