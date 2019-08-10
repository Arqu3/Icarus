﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedActionProvider : BaseActionProvider
{
    GameObject projectilePrefab;
    DamageType damageType;

    public RangedActionProvider(ICombatEntity owner, DamageType damageType, GameObject projectilePrefab) : base(owner)
    {
        this.damageType = damageType;
        this.projectilePrefab = projectilePrefab;
    }

    public override void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K)) owner.GetModifier().ApplyProjectileDecorator(2);
        //else if (Input.GetKeyDown(KeyCode.L)) owner.GetModifier().RemoveProjectileDecorator();

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
        owner.GiveResource(Random.Range(0f, 1f) < 0.2f ? CurrentStatProvider.GetResourceGain() * 2 : CurrentStatProvider.GetResourceGain());
        ShootSpread(CurrentStatProvider.GetProjectileCount());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        ShootSpread(CurrentStatProvider.GetProjectileCount() + 4);

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
        projectile.GetComponent<Projectile>()?.Initialize(damageType, CurrentStatProvider.GetPower(), owner.EntityType);
        projectile.transform.position = origin;
        projectile.transform.rotation = direction;

        Object.Destroy(projectile, 5f);
    }

    public override IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth)
    {
        var data = HeroRangedData.Instance;
        startHealth = data.StartHealth;
        return new RangedStatProvider(mod.power + data.Power, mod.resource + data.ResourceGain, mod.cd + data.ActionCooldown, 1, mod.range + data.Range);
    }
}
