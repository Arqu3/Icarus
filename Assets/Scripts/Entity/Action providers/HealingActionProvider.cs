using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealingActionProvider : BaseActionProvider
{
    const float HealingCooldown = 0.8f;
    float healTimestamp;

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
                if (Target.HealthPercentage >= 0.98f) Target = LookForFriendlyTargetWithLowestHealth();

                if (owner.SpendResourcePercentage(0.99f)) PerformSpecial();
                else if (Target.HealthPercentage < 1f) PerformBasic();
            }
        }
    }

    protected override void PerformBasic()
    {
        if (Time.time - healTimestamp < HealingCooldown) return;

        Target.GiveHealth(8);
        owner.GiveResource(0.15f);

        healTimestamp = Time.time;
    }

    protected override void PerformSpecial()
    {
        Target.GiveHealth(10);
        Target.StartCoroutine(_HealOverTime(2f, 0.5f));
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
                Target.GiveHealth(5);
            }

            yield return null;
        }
    }
}
