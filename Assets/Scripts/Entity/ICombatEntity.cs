using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICombatEntity
{
    int Health { get; }
    int MaxHealth { get; }
    float HealthPercentage { get; }
    void GiveHealth(int amount);
    void GiveHealthPercentage(float percentage);
    DamageResult RemoveHealth(int amount, DamageType type);
    DamageResult RemoveHealthPercentage(float percentage, DamageType type);

    float Resource { get; }
    float ResourcePercentage { get; }
    bool SpendResource(float amount);
    bool SpendResourcePercentage(float percentage);
    void GiveResource(float amount);
    void GiveResourcePercentage(float percentage);

    EntityType EntityType { get; }

    bool Valid { get; }

    void OverrideTarget(ICombatEntity newTarget);

    GameObject gameObject { get; }
    Transform transform { get; }

    Coroutine StartCoroutine(IEnumerator ienumerator);
    void StopCoroutine(IEnumerator ienumerator);

    EntityModifier GetModifier();

    void Stop();
    void Resume();
    Coroutine Stop(float duration);

    bool IsStopped();

    UnityEvent OnDeath { get; }
}

public enum DamageResult
{
    Hit = 0,
    Missed = 1,
    Avoided = 2,
    Blocked = 3,
    Immune = 4
}