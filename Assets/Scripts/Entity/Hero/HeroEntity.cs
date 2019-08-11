using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class HeroEntity : BaseEntity
{
    #region Serialized

    //[Header("Roles")]
    //[SerializeField]
    //HeroRole mainRole = HeroRole.DamageDealer;
    //[SerializeField]
    //HeroRole secondaryRole = HeroRole.None;

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
    }

    protected override void Start()
    {
        base.Start();

        var inventory = FindObjectOfType<BasePlayer>().Inventory;
        for(int i = 0; i < 3; ++i) equipmentSlots.Add(new EquipmentSlot(GetModifier(), inventory));
        //foreach (var s in equipmentSlots)
        //{
        //    var item = ItemCreator.CreateRandomItem();
        //    s.Equip(item);
        //    if (Random.Range(0f, 1f) < 0.33f) s.UnEquip(item);
        //}
    }

    protected override EntityModifier CreateModifier()
    {
        return new EntityModifier(baseHealthProvider, baseResourceProvider, baseStatProvider);
    }

    protected override IActionProvider CreateActionProvider()
    {
        return new MeleeActionProvider(this, damageType);
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

    protected virtual void OnDrawGizmosSelected()
    {
        if (!debug) return;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

#endif

    #endregion
}
