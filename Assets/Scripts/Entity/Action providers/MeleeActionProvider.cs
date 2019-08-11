﻿using System.Collections;
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
        Target.RemoveHealth(CurrentStatProvider.GetPower());
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.RemoveHealth(CurrentStatProvider.GetPower());
        var hits = GetEnemyEntitiesInSphere(owner.transform.position, 5f);
        foreach(var hit in hits)
        {
            hit.RemoveHealth(CurrentStatProvider.GetPower() + 4);
        }

        StartCooldown();
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroMeleeData.Instance;
        startHealth = data.StartHealth;
        return new DefaultStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, mod.range + data.Range);
    }
}
