using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEntity : MonoBehaviour, ICombatEntity
{
    #region Serialized

    [Header("Health")]
    [SerializeField]
    protected int startHealth = 100;

    #endregion

    #region Providers

    protected IEntityHealthProvider healthProvider;
    protected IActionProvider mainAction;
    protected IActionProvider secondaryAction;
    protected IActionProvider currentAction;

    #endregion

    #region Components

    EntityModifier modifier;

    #endregion

    protected virtual void Awake()
    {
        healthProvider = new EntityHealthProvider(startHealth);


        var agent = GetComponent<NavMeshAgent>();
        if (agent) agent.updateUpAxis = false;
    }

    protected virtual void Start()
    {
        modifier = CreateModifier();
    }

    protected virtual void Update()
    {
        currentAction?.Update();
    }

    protected abstract EntityModifier CreateModifier();

    protected virtual void Die()
    {
        Valid = false;
        Destroy(gameObject);
    }

    protected void SwapAction(IActionProvider from, IActionProvider to)
    {
        (currentAction = to)?.OverrideTarget(from?.Target);
    }

    protected virtual void OnHealthRemoveAttempt(DamageResult result)
    {
        if (result == DamageResult.Hit) GetComponent<ObjectFlash>()?.Flash();
        if (healthProvider.GetCurrent() <= 0) Die();
    }

    #region Combat entity interface

    public virtual int Health => healthProvider.GetCurrent();
    public virtual float HealthPercentage => healthProvider.GetPercentage();

    public abstract float Resource { get; }
    public abstract float ResourcePercentage { get; }

    public abstract EntityType EntityType { get; }

    public bool Valid { get; private set; } = true;

    public void GiveHealth(int amount)
    {
        healthProvider.Give(amount);
    }

    public void GiveHealthPercentage(float percentage)
    {
        healthProvider.GivePercentage(percentage);
    }

    public DamageResult RemoveHealth(int amount)
    {
        var result = healthProvider.Remove(amount);
        OnHealthRemoveAttempt(result);
        return result;
    }

    public DamageResult RemoveHealthPercentage(float percentage)
    {
        var result = healthProvider.RemovePercentage(percentage);
        OnHealthRemoveAttempt(result);
        return result;
    }

    public abstract bool SpendResource(float amount);

    public abstract bool SpendResourcePercentage(float percentage);

    public abstract void GiveResource(float amount);

    public abstract void GiveResourcePercentage(float percentage);

    public void OverrideTarget(ICombatEntity newTarget)
    {
        currentAction?.OverrideTarget(newTarget);
    }

    public EntityModifier GetModifier()
    {
        return modifier;
    }

    #endregion
}
