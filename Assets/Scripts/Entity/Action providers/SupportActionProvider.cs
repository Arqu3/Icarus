using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportActionProvider : BaseActionProvider
{
    public SupportActionProvider(ICombatEntity owner) : base(owner)
    {
    }

    public override void Update()
    {
        if (!HasTarget) Target = LookForRandomFriendlyTarget();
        else
        {
            UpdateMovement();

            if (IsInRange && !IsOnCooldown)
            {
                if (owner.SpendResourcePercentage(SpecialResourcePercentageCost)) PerformSpecial();
                else PerformBasic();

                if (Random.Range(0f, 1f) < 0.34f) Target = LookForRandomFriendlyTarget();
            }
        }
    }

    protected override BaseStatProvider CreateStatProvider()
    {
        return new DefaultStatProvider(HeroSupportData.Instance.Power, HeroSupportData.Instance.ResourceGain, HeroSupportData.Instance.ActionCooldown, HeroSupportData.Instance.Range);
    }

    protected override void PerformBasic()
    {
        Target.GiveResource(0.025f);
        owner.GiveResource(statProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.StartCoroutine(_EnergyBurst(1f));

        StartCooldown();
    }

    void GiveEnergyRadius(float radius, float amount)
    {
        var hits = Physics.OverlapSphere(Target.transform.position, radius);

        for(int i = 0; i < hits.Length; ++i)
        {
            var entity = hits[i].GetComponent<BaseEntity>();
            if (entity && entity.EntityType == Target.EntityType)
            {
                entity.GiveResource(amount);
            }
        }
    }

    IEnumerator _EnergyBurst(float waitTime)
    {
        GiveEnergyRadius(4f, 0.05f);

        yield return new WaitForSeconds(waitTime);

        GiveEnergyRadius(6f, 0.15f);
    }
}
