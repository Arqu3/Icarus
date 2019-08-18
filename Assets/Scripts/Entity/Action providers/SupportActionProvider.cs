using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportActionProvider : BaseActionProvider
{
    public SupportActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
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

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroSupportData.Instance;
        startHealth = data.StartHealth;
        return new DefaultStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, mod.range + data.Range);
    }

    protected override void PerformBasic()
    {
        Target.GiveResource(CurrentStatProvider.GetPower() * 0.005f);
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        Target.StartCoroutine(_EnergyBurst(1f));

        StartCooldown();
    }

    void GiveEnergyRadius(float radius, float amount)
    {
        var hits = GetFriendlyEntitiesInSphere(Target.transform.position, radius);

        for(int i = 0; i < hits.Length; ++i)
        {
            hits[i].GiveResource(amount);
        }
    }

    IEnumerator _EnergyBurst(float waitTime)
    {
        GiveEnergyRadius(4f, CurrentStatProvider.GetPower() * 0.01f);

        yield return new WaitForSeconds(waitTime);

        GiveEnergyRadius(6f, CurrentStatProvider.GetPower() * 0.03f);
    }
}
