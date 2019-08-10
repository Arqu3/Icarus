﻿using System.Collections;
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

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroTankData.Instance;
        startHealth = data.StartHealth;
        return new DefaultStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, mod.range + data.Range);
    }

    protected override void PerformBasic()
    {
        Target.RemoveHealth(CurrentStatProvider.GetPower() + 2);
        owner.GiveHealth(CurrentStatProvider.GetPower());
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealthPercentage(0.1f);
        owner.GiveHealthPercentage(0.05f);
        var hits = GetEnemyEntitiesInSphere(owner.transform.position, 7f);
        for(int i = 0; i < hits.Length; ++i)
        {
            owner.GiveHealth(CurrentStatProvider.GetPower());
            hits[i].OverrideTarget(owner);
        }

        StartCooldown();
    }
}
