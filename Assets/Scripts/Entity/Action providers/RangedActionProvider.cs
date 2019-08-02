using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedActionProvider : BaseActionProvider
{
    GameObject projectilePrefab;
    protected BaseRangedStatProvider rangedStatProvider;

    public RangedActionProvider(ICombatEntity owner, DamageType damageType, GameObject projectilePrefab) : base(owner)
    {
        this.projectilePrefab = projectilePrefab;
        rangedStatProvider = statProvider as BaseRangedStatProvider;
    }

    public override void Update()
    {
        if (!HasTarget) Target = LookForRandomEnemyTarget();
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
        owner.GiveResource(Random.Range(0f, 1f) < 0.2f ? rangedStatProvider.GetResourceGain() * 2 : rangedStatProvider.GetResourceGain());
        ShootSpread(rangedStatProvider.GetProjectileCount());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        ShootSpread(rangedStatProvider.GetProjectileCount() + 4);

        StartCooldown();
    }

    void ShootSpread(int projectileCount)
    {
        int iterations = Mathf.Max(1, projectileCount);

        float arc = 80f;
        Vector3 direction = Target.transform.position - owner.transform.position;
        Quaternion rot = Quaternion.Euler(0f, 0, 0f);

        for (int i = 0; i < iterations; ++i)
        {
            if (iterations > 1) rot = Quaternion.Euler(0f, (-arc / 2f) + Mathf.Lerp(0f, arc, (float)i / (iterations - 1)), 0f);
            Vector3 rotatedDirection = rot * direction;

            Shoot(owner.transform.position, Quaternion.LookRotation(rotatedDirection));
        }
    }

    void Shoot(Vector3 origin, Quaternion direction)
    {
        direction = Quaternion.Euler(0f, direction.eulerAngles.y, 0f);
        var projectile = Object.Instantiate(projectilePrefab);
        projectile.GetComponent<Projectile>()?.Initialize(DamageType.Magical, rangedStatProvider.GetPower(), owner.EntityType);
        projectile.transform.position = origin;
        projectile.transform.rotation = direction;

        Object.Destroy(projectile, 5f);
    }

    protected override BaseStatProvider CreateStatProvider()
    {
        return new RangedStatProvider(HeroRangedData.Instance.Power, HeroRangedData.Instance.ResourceGain, HeroRangedData.Instance.ActionCooldown, 1, HeroRangedData.Instance.Range);
    }
}
