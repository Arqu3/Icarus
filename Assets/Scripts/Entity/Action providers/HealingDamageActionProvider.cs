using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealingDamageActionProvider : HealingActionProvider
{
    public HealingDamageActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    public override void Update()
    {
        if (!HasTarget) Target = LookForRandomEnemyTarget();
        else
        {
            UpdateMovement();

            if (IsInRange)
            {
                if (!IsOnCooldown)
                {
                    if (owner.SpendResourcePercentage(SpecialResourcePercentageCost))
                    {
                        Target = LookForFriendlyTargetWithLowestHealth();
                        PerformSpecial();
                    }
                    else PerformBasic();
                }
            }
        }
    }

    protected override void PerformBasic()
    {
        Target.RemoveHealth(CurrentStatProvider.GetPower(), damageType);
        owner.GiveResource(CurrentStatProvider.GetResourceGain());
        owner.GiveHealth(Mathf.CeilToInt(CurrentStatProvider.GetPower() / 2f));
        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.GiveHealth(CurrentStatProvider.GetPower() + 5);
        Target.GiveHealthPercentage(0.05f * CurrentStatProvider.GetPower());

        foreach(var hero in GetFriendlyEntitiesIncludingSelf())
        {
            hero.GiveHealth(CurrentStatProvider.GetPower() + 5);
        }

        Target = LookForRandomEnemyTarget();
        StartCooldown();
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroHealingData.Instance;
        startHealth = data.StartHealth;
        return new DefaultStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, mod.range + data.Range);
    }
}
