using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeRogueActionProvider : MeleeActionProvider
{
    public MeleeRogueActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    protected override void PerformSpecial()
    {
        if (NavMesh.SamplePosition(Target.transform.position - Target.transform.forward, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        Target.RemoveHealth(5 + CurrentStatProvider.GetPower());
        Target.RemoveHealthPercentage(0.015f * CurrentStatProvider.GetPower());
        owner.StartCoroutine(_Evasion(1.5f));

        StartCooldown();
    }

    IEnumerator _Evasion(float duration)
    {
        var deco = new HealthBlockDecorator(null, 2f);
        owner.GetModifier().AddDecorator(deco);

        yield return new WaitForSeconds(duration);

        owner.GetModifier().RemoveDecorator(deco);
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var c = base.CreateBaseStatProvider(mod, out startHealth);
        return new StatRangeConditionDecorator(c as BaseStatProvider, 5f, () => owner.ResourcePercentage > SpecialResourcePercentageCost);
    }
}
