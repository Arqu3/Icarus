using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTankActionProvider : TankActionProvider
{
    public SupportTankActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    public override void Update()
    {
        if (!HasTarget) Target = LookForNearestEnemy();
        else
        {
            UpdateMovement();

            if (!IsOnCooldown)
            {
                if (owner.SpendResourcePercentage(SpecialResourcePercentageCost)) PerformSpecial();
                else if (IsInRange) PerformBasic();
            }
        }
    }

    protected override void PerformBasic()
    {
        Target.RemoveHealth(CurrentStatProvider.GetPower() - 3, damageType);

        var hits = GetFriendlyEntitiesInSphere(owner.transform.position, 3f);
        foreach (var hit in hits)
        {
            if (hit == owner) continue;
            hit.GiveResourcePercentage(0.01f * CurrentStatProvider.GetPower());
        }

        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        var hits = GetFriendlyEntitiesInSphere(owner.transform.position, 7f);
        foreach(var hit in hits)
        {
            if (hit == owner || hit as HeroSupportTank) continue;

            hit.StartCoroutine(_ApplyDecorator(hit, 3f));
        }

        var enemyHits = GetEnemyEntitiesInSphere(owner.transform.position, 7f);
        foreach (var hit in enemyHits) hit.OverrideTarget(owner);

        StartCooldown();
    }

    IEnumerator _ApplyDecorator(ICombatEntity entity, float duration)
    {
        var dec = new SingleStatDecorator(null, StatType.Resource, ModMathType.Multiplicative, 1.2f);
        entity.GetModifier().AddDecorator(dec, ModMathType.Multiplicative);

        var hpDec = new HealthRedirectDecorator(null, owner, 0.4f);
        entity.GetModifier().AddDecorator(hpDec);

        yield return new WaitForSeconds(duration);

        entity.GetModifier().RemoveDecorator(dec);
        entity.GetModifier().RemoveDecorator(hpDec);
    }
}
