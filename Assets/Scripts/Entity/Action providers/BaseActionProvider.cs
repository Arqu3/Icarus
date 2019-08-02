using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public abstract class BaseActionProvider : IActionProvider
{
    [System.Obsolete("cyclic, rework later")]
    protected ICombatEntity owner;

    protected float range;
    protected NavMeshAgent agent;

    protected float actionTimestamp;

    public BaseActionProvider(ICombatEntity owner, float range)
    {
        this.range = range;
        this.owner = owner;
        agent = owner.gameObject.GetComponent<NavMeshAgent>();
    }

    public bool HasTarget => Target != null && Target.Valid;
    public ICombatEntity Target { get; protected set; }

    public bool IsInRange => HasTarget ? Vector3.Distance(Target.transform.position, owner.transform.position) < range : false;

    public void OverrideTarget(ICombatEntity target)
    {
        Target = target;
    }

    public abstract void Update();
    protected abstract void PerformBasic();
    protected abstract void PerformSpecial();

    #region Cooldown

    protected abstract float ActionCooldown { get; }
    protected void StartCooldown()
    {
        actionTimestamp = Time.time;
    }
    protected bool IsOnCooldown => Time.time - actionTimestamp < ActionCooldown;

    #endregion

    #region Power/resource

    protected abstract int Power { get; }
    protected abstract float ResourceGain { get; }
    protected virtual float SpecialResourcePercentageCost => 0.99f;

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
            if (Vector3.Distance(owner.transform.position, Target.transform.position) > range)
            {
                if (agent.isStopped) agent.isStopped = false;
                agent.SetDestination(Target.transform.position);
            }
            else if (!agent.isStopped) agent.isStopped = true;
        }
    }

    #endregion
}
