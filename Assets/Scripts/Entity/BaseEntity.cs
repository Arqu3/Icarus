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

    protected IEntityHealthProvider baseHealthProvider;
    protected IActionProvider mainAction;
    protected IActionProvider secondaryAction;
    protected IActionProvider currentAction;

    #endregion

    #region Private

    EntityModifier modifier;
    ObjectFlash flash;
    NavMeshAgent agent;
    int locks;

    #endregion

    protected virtual void Awake()
    {
        baseHealthProvider = new EntityHealthProvider(startHealth);
        flash = GetComponent<ObjectFlash>();

        agent = GetComponent<NavMeshAgent>();
        if (agent) agent.updateUpAxis = false;
    }

    protected virtual void Start()
    {
        modifier = CreateModifier();
    }

    protected virtual void Update()
    {
        if (IsStopped()) return;

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

    public void GiveHealth(int amount)
    {
        CurrentHealthProvider.Give(amount);
    }

    public void GiveHealthPercentage(float percentage)
    {
        CurrentHealthProvider.GivePercentage(percentage);
    }

    public virtual DamageResult RemoveHealth(int amount)
    {
        var result = CurrentHealthProvider.Remove(amount);
        OnHealthRemoveAttempt(result);
        return result;
    }

    public virtual DamageResult RemoveHealthPercentage(float percentage)
    {
        var result = CurrentHealthProvider.RemovePercentage(percentage);
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
