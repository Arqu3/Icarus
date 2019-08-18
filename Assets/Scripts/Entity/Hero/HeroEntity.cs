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

    int numTimesDowned = 0;

    List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    #endregion

    void Start()
    {
        //var inventory = FindObjectOfType<BasePlayer>().Inventory;
        for (int i = 0; i < Hero.ITEMSLOTS; ++i) equipmentSlots.Add(new EquipmentSlot(GetModifier()));//, inventory));
    }

    IEnumerator _GetDowned()
    {
        Valid = false;
        Downed = true;

        agent.isStopped = true;

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

    public Coroutine EquipItemsDelayed(List<EquipableItem> items)
    {
        return StartCoroutine(_EquipItemsDelayed(items));
    }

    IEnumerator _EquipItemsDelayed(List<EquipableItem> items)
    {
        yield return new WaitWhile(() => equipmentSlots.Count <= 0);

        for (int i = 0; i < items.Count; ++i) equipmentSlots[i].Equip(items[i]);

        GiveHealthPercentage(1f);
    }

    protected override IEntityResourceProvider CreateResourceProvider()
    {
        return new EntityResourceProvider(1f);
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

    public override DamageResult RemoveHealth(int amount, DamageType type)
    {
        if (Valid) return base.RemoveHealth(amount, type);
        else return DamageResult.Immune;
    }

    public override DamageResult RemoveHealthPercentage(float percentage, DamageType type)
    {
        if (Valid) return base.RemoveHealthPercentage(percentage, type);
        else return DamageResult.Immune;
    }

    #endregion

    public bool Downed { get; protected set; } = false;

    public EquipmentSlot[] EquipmentSlots => equipmentSlots.ToArray();

    #region Description

    const string BaseDescription =
    "{0} - Level {1} {2}\n" +
    "{3}\n" +
    "Base health: {4}\n\n";

    public string GetDescription(string heroName, int level)
    {
        return string.Format(BaseDescription + GetAdditionalDescription(), heroName, level, gameObject.name, GetClassType(), GetStartHealth().ToString());
    }

    protected abstract string GetAdditionalDescription();
    protected abstract int GetStartHealth();
    protected abstract string GetClassType();

    #endregion

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
