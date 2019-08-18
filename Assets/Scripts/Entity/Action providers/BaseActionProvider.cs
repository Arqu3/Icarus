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
    protected virtual IStatProvider CurrentStatProvider => owner.GetModifier().GetCurrentStatProvider();
    public const float MinimumActionCooldown = 0.1f;
    protected DamageType damageType;

    public BaseActionProvider(ICombatEntity owner, DamageType damageType)
    {
        this.owner = owner;
        this.damageType = damageType;
        agent = owner.gameObject.GetComponent<NavMeshAgent>();

        actionTimestamp = Time.time + 1f;
    }

    public bool HasTarget => Target != null && Target.Valid;
    public ICombatEntity Target { get; protected set; }

    public bool IsInRange => HasTarget ? Vector3.Distance(Target.transform.position, owner.transform.position) < CurrentStatProvider.GetRange() : false;

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
    protected bool IsOnCooldown => Time.time - actionTimestamp < Mathf.Max(CurrentStatProvider.GetActionCooldown(), MinimumActionCooldown);

    #endregion

    #region Power/resource

    public abstract IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth);
    protected virtual float SpecialResourcePercentageCost => 0.99f;

    #endregion

    #region Targeting help functions

    protected ICombatEntity[] GetEnemyEntitiesInSphere(Vector3 position, float range)
        => (from h
            in Physics.OverlapSphere(position, range)
            let e = h.GetComponent<ICombatEntity>()
            where e != null && e.EntityType != owner.EntityType
            select e).ToArray();

    protected ICombatEntity[] GetFriendlyEntitiesInSphere(Vector3 position, float range)
        => (from h
            in Physics.OverlapSphere(position, range)
            let e = h.GetComponent<ICombatEntity>()
            where e != null && e.EntityType == owner.EntityType
            select e).ToArray();

    protected ICombatEntity[] GetFriendlyEntitiesIncludingSelf()
    {
        return (from t in TargetProvider.Get() where t.EntityType == owner.EntityType select t).ToArray();
    }

    protected ICombatEntity[] GetFriendlyEntities()
    {
        return (from t in TargetProvider.Get() where (ICombatEntity)t != owner && t.EntityType == owner.EntityType select t).ToArray();
    }

    protected ICombatEntity[] GetEnemyEntities()
    {
        return (from t in TargetProvider.Get() where (ICombatEntity)t != owner && t.EntityType != owner.EntityType select t).ToArray();
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
            if (Vector3.Distance(owner.transform.position, Target.transform.position) > CurrentStatProvider.GetRange())
            {
                if (agent.isStopped) agent.isStopped = false;
                agent.SetDestination(Target.transform.position);
            }
            else if (!agent.isStopped) agent.isStopped = true;
        }
    }

    #endregion
}
