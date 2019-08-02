using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedActionProvider : BaseActionProvider
{
    GameObject projectilePrefab;

    protected override float ActionCooldown => HeroRangedData.Instance.ActionCooldown;
    protected override int Power => HeroRangedData.Instance.Power;
    protected override float ResourceGain => Random.Range(0f, 1f) < 0.2f ? HeroRangedData.Instance.ResourceGain * 2 : HeroRangedData.Instance.ResourceGain;

    public RangedActionProvider(ICombatEntity owner, float range, DamageType damageType, GameObject projectilePrefab) : base(owner, range)
    {
        this.projectilePrefab = projectilePrefab;
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
        owner.GiveResource(ResourceGain);
        Shoot();

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        int iterations = 5;

        float arc = 80f;
        float arcIteration = arc / iterations;

        for (int i = 0; i < iterations; ++i)
        {
            Vector3 direction = Target.transform.position - owner.transform.position;
            Vector3 rotatedDirection = Quaternion.Euler(0f, (-arc / 2f) + arcIteration * i, 0f) * direction;
            Shoot(owner.transform.position + rotatedDirection.normalized * 1.5f, Quaternion.LookRotation(rotatedDirection));
        }

        StartCooldown();
    }

    void Shoot()
    {
        Shoot(owner.transform.position + (Target.transform.position - owner.transform.position).normalized * 1.5f, Quaternion.LookRotation(Target.transform.position - owner.transform.position));
    }

    void Shoot(Vector3 origin, Quaternion direction)
    {
        direction = Quaternion.Euler(0f, direction.eulerAngles.y, 0f);
        var projectile = Object.Instantiate(projectilePrefab);
        projectile.GetComponent<Projectile>()?.Initialize(DamageType.Magical, Power, owner.EntityType);
        projectile.transform.position = origin;
        projectile.transform.rotation = direction;

        Object.Destroy(projectile, 5f);
    }
}
