using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealingActionProvider : BaseActionProvider
{
    protected override float ActionCooldown => 0.8f;

    protected override int Power => 8;
    protected override float ResourceGain => 0.15f;

    const float TargetUpdateCooldown = 4f;
    float targetUpdateTimer = 0f;

    public HealingActionProvider(ICombatEntity owner, float range, DamageType damageType) : base(owner, range)
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

    protected override void PerformBasic()
    {
        Target.GiveHealth(Power);
        owner.GiveResource(ResourceGain);

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.GiveHealth(Power + 2);
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
                Target.GiveHealth((Power / 2) + 1);
            }

            yield return null;
        }
    }
}
