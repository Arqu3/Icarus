using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankActionProvider : BaseActionProvider
{
    public TankActionProvider(ICombatEntity owner) : base(owner)
    {
    }

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

    public override BaseStatProvider CreateBaseStatProvider()
    {
        return new DefaultStatProvider(HeroTankData.Instance.Power, HeroTankData.Instance.ResourceGain, HeroTankData.Instance.ActionCooldown, HeroTankData.Instance.Range);
    }

    protected override void PerformBasic()
    {
        Target.RemoveHealthPercentage(0.01f);
        owner.GiveHealth(CurrentStatProvider.GetPower());
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

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
                owner.GiveHealth(CurrentStatProvider.GetPower());
                entity.OverrideTarget(owner);
            }
        }

        StartCooldown();
    }
}
