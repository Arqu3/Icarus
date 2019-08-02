using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankActionProvider : BaseActionProvider
{
    public TankActionProvider(ICombatEntity owner, float range) : base(owner, range)
    {
    }

    protected override float ActionCooldown => HeroTankData.Instance.ActionCooldown;

    protected override int Power => HeroTankData.Instance.Power;

    protected override float ResourceGain => HeroTankData.Instance.ResourceGain;

    public override void Update()
    {
        if (!HasTarget) Target = LookForNearestEnemy();
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
        Target.RemoveHealthPercentage(0.01f);
        owner.GiveHealth(Power);
        owner.GiveResource(ResourceGain);

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealthPercentage(0.02f);
        owner.GiveHealthPercentage(0.05f);
        var hits = Physics.OverlapSphere(owner.transform.position, 7f);
        for(int i = 0; i < hits.Length; ++i)
        {
            var entity = hits[i].GetComponent<BaseEntity>();
            if (entity && entity.EntityType != owner.EntityType)
            {
                owner.GiveHealth(Power);
                entity.OverrideTarget(owner);
            }
        }

        StartCooldown();
    }
}
