using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyEntity : BaseEntity
{
    [Header("Type")]
    [SerializeField]
    HeroRole role = HeroRole.DamageDealer;
    [SerializeField]
    AttackType attackType = AttackType.Melee;
    [SerializeField]
    [ConditionalField(nameof(attackType), AttackType.Ranged)]
    GameObject projectilePrefab;
    [SerializeField]
    DamageType damageType = DamageType.Physical;

    protected override void Awake()
    {
        base.Awake();
        switch (role)
        {
            case HeroRole.None:
                break;
            case HeroRole.Tank:
                break;
            case HeroRole.Support:
                break;
            case HeroRole.DamageDealer:

                switch (attackType)
                {
                    case AttackType.Melee:
                        mainAction = new EnemyMeleeActionProvider(this, damageType);
                        break;
                    case AttackType.Ranged:
                        mainAction = new EnemyRangedActionProvider(this, damageType, projectilePrefab);
                        break;
                    default:
                        break;
                }

                break;
            case HeroRole.Healer:
                break;
            default:
                break;
        }
        currentAction = mainAction;
    }

    protected override EntityModifier CreateModifier()
    {
        return new EntityModifier((BaseEntityHealthProvider)healthProvider, null, mainAction.GetBaseStatProvider());
    }

    #region Combat entity interface

    public override float Resource => 0f;

    public override float ResourcePercentage => 0f;

    public override EntityType EntityType => EntityType.Enemy;

    public override void GiveResource(float amount){}

    public override void GiveResourcePercentage(float percentage) { }

    public override bool SpendResource(float amount)
    {
        return false;
    }

    public override bool SpendResourcePercentage(float percentage)
    {
        return false;
    }

    #endregion
}
