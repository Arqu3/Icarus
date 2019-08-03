using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealingActionProvider : BaseActionProvider
{
    const float TargetUpdateCooldown = 4f;
    float targetUpdateTimer = 0f;

    public HealingActionProvider(ICombatEntity owner, DamageType damageType) : base(owner)
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

    protected override BaseStatProvider CreateStatProvider()
    {
        return new DefaultStatProvider(HeroHealingData.Instance.Power, HeroHealingData.Instance.ResourceGain, HeroHealingData.Instance.ActionCooldown, HeroHealingData.Instance.Range);
    }

    protected override void PerformBasic()
    {
        Target.GiveHealth(baseStatProvider.GetPower());
        owner.GiveResource(baseStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.GiveHealth(baseStatProvider.GetPower() + 2);
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
                Target.GiveHealth((baseStatProvider.GetPower() / 2) + 1);
            }

            yield return null;
        }
    }
}
