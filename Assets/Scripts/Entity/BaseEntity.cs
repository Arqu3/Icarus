using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent), typeof(ObjectFlash))]
public abstract class BaseEntity : MonoBehaviour, ICombatEntity
{
    #region Serialized

    //[Header("Health")]
    //[SerializeField]
    //protected int startHealth = 100;

    [Header("Start stat mods")]
    [SerializeField]
    protected int startHealthAdd = 0;
    [SerializeField]
    protected int startPowerAdd = 0;
    [SerializeField]
    protected float startCDAdd = 0f;
    [SerializeField]
    protected float startResourceAdd = 0f;
    [SerializeField]
    protected float startRangeAdd = 0f;

    [Header("Attack")]
    [SerializeField]
    protected AttackType attackType = AttackType.Melee;
    [SerializeField]
#if UNITY_EDITOR
    [ConditionalField(nameof(attackType), AttackType.Ranged)]
#endif
    protected Projectile projectilePrefab;
    [SerializeField]
    protected DamageType damageType = DamageType.Physical;

    #endregion

    #region Providers

    protected IActionProvider mainAction;
    protected IActionProvider secondaryAction;
    protected IActionProvider currentAction;

    #endregion

    #region Events

    private readonly UnityEvent onDeath = new UnityEvent();

    #endregion

    #region Private

    EntityModifier modifier;
    ObjectFlash flash;
    protected NavMeshAgent agent;
    int locks;
    float regenTimestamp = 0f;

    #endregion

    public struct StatMultipliers
    {
        public int hp;
        public int power;
        public float resource;
        public float cd;
        public float range;
    }

    protected virtual void Awake()
    {
        TargetProvider.Add(this);

        flash = GetComponent<ObjectFlash>();
        agent = GetComponent<NavMeshAgent>();
        if (agent) agent.updateUpAxis = false;

        mainAction = currentAction = CreateActionProvider();
        modifier = CreateModifier();
    }

    protected virtual void OnDestroy()
    {
        TargetProvider.Remove(this);
    }

    protected virtual void Update()
    {
        if (Time.time - regenTimestamp > CurrentHealthProvider.GetRegenInterval())
        {
            GiveHealth(CurrentHealthProvider.GetRegenAmount());
            regenTimestamp = Time.time;
        }

        if (IsStopped() || !Valid) return;

        currentAction?.Update();
    }

    protected abstract IActionProvider CreateActionProvider();
    EntityModifier CreateModifier()
    {
        var stat = CreateStatProvider(out int health);
        return new EntityModifier(CreateHealthProvider(health), CreateResourceProvider(), stat);
    }

    protected abstract IEntityResourceProvider CreateResourceProvider();
    protected virtual IEntityHealthProvider CreateHealthProvider(int startHealth) => new EntityHealthProvider(startHealth, () => CurrentHealthProvider);
    protected virtual IStatProvider CreateStatProvider(out int health)
    {
        var stat = mainAction.CreateBaseStatProvider(new StatMultipliers
        {
            cd = startCDAdd,
            hp = startHealthAdd,
            power = startPowerAdd,
            range = startRangeAdd,
            resource = startResourceAdd
        },
        out int startHealth);

        health = startHealth;
        return stat;
    }

    protected virtual void Die()
    {
        Valid = false;
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    protected void SwapAction(IActionProvider from, IActionProvider to)
    {
        (currentAction = to)?.OverrideTarget(from?.Target);
    }

    protected virtual void OnHealthRemoveAttempt(DamageResult result)
    {
        if (result == DamageResult.Hit) flash?.Flash();
        if (CurrentHealthProvider.GetCurrent() <= 0) Die();
    }

    protected IEntityHealthProvider CurrentHealthProvider => GetModifier()?.GetCurrentHealthProvider();
    protected IEntityResourceProvider CurrentResourceProvider => GetModifier()?.GetCurrentResourceProvider();

    #region Combat entity interface

    public virtual int MaxHealth => CurrentHealthProvider != null ? CurrentHealthProvider.GetMax() : 0;
    public virtual int Health => CurrentHealthProvider != null ? CurrentHealthProvider.GetCurrent() : 0;
    public virtual float HealthPercentage => CurrentHealthProvider != null ? CurrentHealthProvider.GetPercentage() : 0f;
    public abstract float Resource { get; }
    public abstract float ResourcePercentage { get; }

    public abstract EntityType EntityType { get; }

    public bool Valid { get; protected set; } = true;

    public UnityEvent OnDeath => onDeath;

    public void GiveHealth(int amount)
    {
        CurrentHealthProvider.Give(amount);
    }

    public void GiveHealthPercentage(float percentage)
    {
        CurrentHealthProvider.GivePercentage(percentage);
    }

    public virtual DamageResult RemoveHealth(int amount, DamageType type)
    {
        var result = CurrentHealthProvider.Remove(amount, type);
        OnHealthRemoveAttempt(result);
        return result;
    }

    public virtual DamageResult RemoveHealthPercentage(float percentage, DamageType type)
    {
        var result = CurrentHealthProvider.RemovePercentage(percentage, type);
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

    public void Stop()
    {
        locks++;
        if (agent) agent.isStopped = true;
    }

    public void Resume()
    {
        locks = Mathf.Clamp(locks - 1, 0, locks + 1);
        if (agent) agent.isStopped = IsStopped();
    }

    public bool IsStopped()
    {
        return locks > 0;
    }

    public Coroutine Stop(float duration)
    {
        return StartCoroutine(_Stop(duration));
    }

    IEnumerator _Stop(float duration)
    {
        Stop();
        yield return new WaitForSeconds(duration);
        Resume();
    }

    #endregion
}
