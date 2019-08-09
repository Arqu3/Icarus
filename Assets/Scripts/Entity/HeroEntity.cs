using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroEntity : BaseEntity
{
    #region Serialized

    [Header("Roles")]
    [SerializeField]
    HeroRole mainRole = HeroRole.DamageDealer;
    //[SerializeField]
    //HeroRole secondaryRole = HeroRole.None;

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

    [Header("Down times")]
    [SerializeField]
    int timesDownedBeforeDeath = 3;
    [SerializeField]
    float downTime = 2f;
    [SerializeField]
    float healthPercentageToReturn = 0.33f;

    #endregion

    #region Private

    IEntityResourceProvider baseResourceProvider;
    int numTimesDowned = 0;

    List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    #endregion

    protected override void Awake()
    {
        base.Awake();

        baseResourceProvider = new EntityResourceProvider(1f);

        switch (mainRole)
        {
            case HeroRole.None:
                break;
            case HeroRole.Tank:
                mainAction = new TankActionProvider(this);
                baseHealthProvider = new HealthBlockDecorator((BaseEntityHealthProvider)baseHealthProvider, 0.2f);
                break;
            case HeroRole.Support:
                mainAction = new SupportActionProvider(this);
                break;
            case HeroRole.DamageDealer:

                switch (attackType)
                {
                    case AttackType.Melee:
                        mainAction = new MeleeRogueActionProvider(this, damageType);
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

        //if (secondaryRole != mainRole)
        //{
        //    switch (secondaryRole)
        //    {
        //        case HeroRole.None:
        //            break;
        //        case HeroRole.Tank:
        //            break;
        //        case HeroRole.Support:
        //            break;
        //        case HeroRole.DamageDealer:
        //            break;
        //        case HeroRole.Healer:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        currentAction = mainAction;
    }

    protected override void Start()
    {
        base.Start();

        for(int i = 0; i < 3; ++i) equipmentSlots.Add(new EquipmentSlot(GetModifier(), FindObjectOfType<BasePlayer>().Inventory));
        foreach (var s in equipmentSlots)
        {
            var item = ItemCreator.CreateRandomItem();
            s.Equip(item);
            //if (Random.Range(0f, 1f) < 0.33f) s.UnEquip(item);
        }
    }

    protected override EntityModifier CreateModifier()
    {
        return new EntityModifier(baseHealthProvider, baseResourceProvider, mainAction.CreateBaseStatProvider());
    }

    IEnumerator _GetDowned()
    {
        Valid = false;
        Downed = true;

        GetComponent<NavMeshAgent>().isStopped = true;

        float timer = 0.0f;

        while (timer < downTime)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        GiveHealthPercentage(healthPercentageToReturn);

        Downed = false;
        Valid = true;
    }

    #region Combat entity interface

    protected override void Die()
    {
        if (numTimesDowned >= timesDownedBeforeDeath) base.Die();
        else
        {
            ++numTimesDowned;

            StartCoroutine(_GetDowned());
        }
    }

    public override float Resource => CurrentResourceProvider != null ? CurrentResourceProvider.GetCurrent() : 0f;

    public override float ResourcePercentage => CurrentResourceProvider != null ? CurrentResourceProvider.GetPercentage() : 0f;

    public override EntityType EntityType => EntityType.Friendly;

    public override bool SpendResource(float amount)
    {
        return CurrentResourceProvider.Spend(amount);
    }

    public override bool SpendResourcePercentage(float percentage)
    {
        return CurrentResourceProvider.SpendPercentage(percentage);
    }

    public override void GiveResource(float amount)
    {
        CurrentResourceProvider.Give(amount);
    }

    public override void GiveResourcePercentage(float percentage)
    {
        CurrentResourceProvider.GivePercentage(percentage);
    }

    public override DamageResult RemoveHealth(int amount)
    {
        if (Valid) return base.RemoveHealth(amount);
        else return DamageResult.Immune;
    }

    public override DamageResult RemoveHealthPercentage(float percentage)
    {
        if (Valid) return base.RemoveHealthPercentage(percentage);
        else return DamageResult.Immune;
    }

    #endregion

    public bool Downed { get; protected set; } = false;

    public EquipmentSlot[] EquipmentSlots => equipmentSlots.ToArray();

    #region DEBUG

#if UNITY_EDITOR

    [Header("EDITOR ONLY DEBUG")]
    [SerializeField]
    bool debug = false;
    [SerializeField]
    float radius = 1f;

    private void OnDrawGizmosSelected()
    {
        if (!debug) return;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

#endif

    #endregion
}
