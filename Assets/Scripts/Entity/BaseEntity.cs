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

    #endregion

    protected virtual void Awake()
    {
        healthProvider = new EntityHealthProvider(startHealth);

        var agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            Debug.Log(gameObject.name + " Updated navmesh agent settings");
        }
    }

    protected virtual void Update()
    {
        currentAction?.Update();
    }

    protected virtual void Die()
    {
        Valid = false;
        Destroy(gameObject);
    }

    protected void SwapAction(IActionProvider from, IActionProvider to)
    {
        (currentAction = to)?.OverrideTarget(from?.Target);
    }

    #region Combat entity interface

    public virtual int Health => healthProvider.Current;
    public virtual float HealthPercentage => healthProvider.Percentage;

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

    public void RemoveHealth(int amount)
    {
        healthProvider.Remove(amount);
        if (healthProvider.Current <= 0) Die();
    }

    public void RemoveHealthPercentage(float percentage)
    {
        healthProvider.RemovePercentage(percentage);
        if (healthProvider.Current <= 0) Die();
    }

    public abstract bool SpendResource(float amount);

    public abstract bool SpendResourcePercentage(float percentage);

    public abstract void GiveResource(float amount);

    public abstract void GiveResourcePercentage(float percentage);

    public void OverrideTarget(ICombatEntity newTarget)
    {
        currentAction?.OverrideTarget(newTarget);
    }

    #endregion
}
