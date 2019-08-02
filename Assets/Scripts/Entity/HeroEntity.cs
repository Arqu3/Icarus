using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEntity : BaseEntity
{
    #region Serialized

    [Header("Roles")]
    [SerializeField]
    HeroRole mainRole = HeroRole.DamageDealer;
    [SerializeField]
    HeroRole secondaryRole = HeroRole.None;

    [Header("Attack")]
    [SerializeField]
    AttackType attackType = AttackType.Melee;
    [SerializeField]
    #if UNITY_EDITOR
    [ConditionalField(nameof(attackType), AttackType.Ranged)]
    #endif
    GameObject projectilePrefab;
    [SerializeField]
    DamageType damageType = DamageType.Physical;

    #endregion

    #region Private

    IEntityResourceProvider resourceProvider;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        resourceProvider = new EntityResourceProvider(1f);

        resourceProvider = new ResourceDecorator((BaseEntityResourceProvider)resourceProvider);

        switch (mainRole)
        {
            case HeroRole.None:
                break;
            case HeroRole.Tank:
                mainAction = new TankActionProvider(this);
                break;
            case HeroRole.Support:
                mainAction = new SupportActionProvider(this);
                break;
            case HeroRole.DamageDealer:

                switch (attackType)
                {
                    case AttackType.Melee:
                        mainAction = new MeleeActionProvider(this, damageType);
                        break;
                    case AttackType.Ranged:
                        mainAction = new RangedActionProvider(this, damageType, projectilePrefab);
                        break;
                    default:
                        break;
                }

                break;
            case HeroRole.Healer:
                mainAction = new HealingActionProvider(this, damageType);
                break;
            default:
                break;
        }

        switch (secondaryRole)
        {
            case HeroRole.None:
                break;
            case HeroRole.Tank:
                break;
            case HeroRole.Support:
                break;
            case HeroRole.DamageDealer:
                break;
            case HeroRole.Healer:
                break;
            default:
                break;
        }

        currentAction = mainAction;
    }

    #region Combat entity interface

    public override float Resource => resourceProvider.GetCurrent();

    public override float ResourcePercentage => resourceProvider.GetPercentage();

    public override EntityType EntityType => EntityType.Friendly;

    public override bool SpendResource(float amount)
    {
        return resourceProvider.Spend(amount);
    }

    public override bool SpendResourcePercentage(float percentage)
    {
        return resourceProvider.SpendPercentage(percentage);
    }

    public override void GiveResource(float amount)
    {
        resourceProvider.Give(amount);
    }

    public override void GiveResourcePercentage(float percentage)
    {
        resourceProvider.GivePercentage(percentage);
    }

    #endregion
}
