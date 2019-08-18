using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealingActionProvider : BaseActionProvider
{
    const float TargetUpdateCooldown = 4f;
    float targetUpdateTimer = 0f;

    public HealingActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
        
    }

    public override void Update()
    {
        targetUpdateTimer += Time.deltaTime;
        if (targetUpdateTimer >= TargetUpdateCooldown)
        {
            Target = LookForFriendlyTargetWithLowestHealth();
            targetUpdateTimer = 0f;
        }

        if (!HasTarget) Target = LookForFriendlyTargetWithLowestHealth();
        else
        {
            UpdateMovement();

            if (IsInRange)
            {
                if (Target.HealthPercentage >= 0.95f) Target = LookForFriendlyTargetWithLowestHealth();

                if (!IsOnCooldown)
                {
                    if (owner.SpendResourcePercentage(SpecialResourcePercentageCost)) PerformSpecial();
                    else if (Target.HealthPercentage < 1f) PerformBasic();
                }
            }
        }
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroHealingData.Instance;
        startHealth = data.StartHealth;
        return new DefaultStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, mod.range + data.Range);
    }

    protected override void PerformBasic()
    {
        Target.GiveHealth(CurrentStatProvider.GetPower());
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.GiveHealth(CurrentStatProvider.GetPower() + 2);
        Target.GiveHealthPercentage(0.1f);
        Target.StartCoroutine(_HealOverTime(2f, 0.5f));

        StartCooldown();
    }

    IEnumerator _HealOverTime(float duration, float interval)
    {
        float timer = 0.0f;
        float intervalTimer = 0.0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;

            intervalTimer += Time.deltaTime;
            if (intervalTimer >= interval)
            {
                intervalTimer = 0.0f;
                Target.GiveHealth((CurrentStatProvider.GetPower() / 2) + 1);
            }

            yield return null;
        }
    }
}
