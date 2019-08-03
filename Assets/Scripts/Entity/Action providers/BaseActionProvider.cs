using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public abstract class BaseActionProvider : IActionProvider
{
    [System.Obsolete("cyclic, rework later")]
    protected ICombatEntity owner;

    protected NavMeshAgent agent;

    protected float actionTimestamp;

    protected BaseStatProvider baseStatProvider;

    public BaseActionProvider(ICombatEntity owner)
    {
        this.owner = owner;
        agent = owner.gameObject.GetComponent<NavMeshAgent>();
        baseStatProvider = CreateStatProvider();
    }

    public bool HasTarget => Target != null && Target.Valid;
    public ICombatEntity Target { get; protected set; }

    public bool IsInRange => HasTarget ? Vector3.Distance(Target.transform.position, owner.transform.position) < baseStatProvider.GetRange() : false;

    public void OverrideTarget(ICombatEntity target)
    {
        Target = target;
    }

    public abstract void Update();
    protected abstract void PerformBasic();
    protected abstract void PerformSpecial();

    #region Cooldown

    protected void StartCooldown()
    {
        actionTimestamp = Time.time;
    }
    protected bool IsOnCooldown => Time.time - actionTimestamp < baseStatProvider.GetActionCooldown();

    #endregion

    #region Power/resource

    protected abstract BaseStatProvider CreateStatProvider();
    protected virtual float SpecialResourcePercentageCost => 0.99f;
    public BaseStatProvider GetBaseStatProvider()
    {
        return baseStatProvider;
    }

    #endregion

    #region Targeting help functions

    protected BaseEntity[] GetFriendlyEntitiesIncludingSelf()
    {
        return (from t in Object.FindObjectsOfType<BaseEntity>() where t.EntityType == owner.EntityType select t).ToArray();
    }

    protected BaseEntity[] GetFriendlyEntities()
    {
        return (from t in Object.FindObjectsOfType<BaseEntity>() where (ICombatEntity)t != owner && t.EntityType == owner.EntityType select t).ToArray();
    }

    protected BaseEntity[] GetEnemyEntities()
    {
        return (from t in Object.FindObjectsOfType<BaseEntity>() where (ICombatEntity)t != owner && t.EntityType != owner.EntityType select t).ToArray();
    }

    protected ICombatEntity LookForNearestEnemy()
    {
        var collection = GetEnemyEntities();
        return collection.OrderBy(x => Vector3.Distance(x.transform.position, owner.transform.position)).FirstOrDefault();
    }

    protected ICombatEntity LookForRandomEnemyTarget()
    {
        var collection = GetEnemyEntities();
        if (collection.Length <= 0) return null;
        return collection.Random();
    }

    protected ICombatEntity LookForRandomFriendlyTarget()
    {
        var collection = GetFriendlyEntities();
        if (collection.Length <= 0) return null;
        return collection.Random();
    }

    protected ICombatEntity LookForFriendlyTargetWithLowestHealth()
    {
        return GetFriendlyEntitiesIncludingSelf().OrderBy(x => x.HealthPercentage).FirstOrDefault();
    }

    #endregion

    #region Movement help functions

    protected virtual void UpdateMovement()
    {
        if (HasTarget)
        {
            if (Vector3.Distance(owner.transform.position, Target.transform.position) > baseStatProvider.GetRange())
            {
                if (agent.isStopped) agent.isStopped = false;
                agent.SetDestination(Target.transform.position);
            }
            else if (!agent.isStopped) agent.isStopped = true;
        }
    }

    #endregion
}
