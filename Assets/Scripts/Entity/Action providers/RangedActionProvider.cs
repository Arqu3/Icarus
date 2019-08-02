using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedActionProvider : BaseActionProvider
{
    GameObject projectilePrefab;

    const float ShootCooldown = 0.9f;
    float shootTimestamp;

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

            if (IsInRange)
            {
                if (owner.SpendResourcePercentage(0.99f)) PerformSpecial();
                else PerformBasic();
            }
        }
    }

    protected override void PerformBasic()
    {
        if (Time.time - shootTimestamp < ShootCooldown) return;

        owner.GiveResource(Random.Range(0f, 1f) < 0.2f ? 0.2f : 0.05f);
        Shoot();

        shootTimestamp = Time.time;
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
    }

    void Shoot()
    {
        Shoot(owner.transform.position + (Target.transform.position - owner.transform.position).normalized * 1.5f, Quaternion.LookRotation(Target.transform.position - owner.transform.position));
    }

    void Shoot(Vector3 origin, Quaternion direction)
    {
        var projectile = Object.Instantiate(projectilePrefab);
        projectile.GetComponent<Projectile>()?.Initialize(DamageType.Magical, 20, owner.EntityType);
        projectile.transform.position = origin;
        projectile.transform.rotation = direction;

        Object.Destroy(projectile, 5f);
    }
}
