using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatEntity
{
    int Health { get; }
    float HealthPercentage { get; }
    void GiveHealth(int amount);
    void GiveHealthPercentage(float percentage);
    DamageResult RemoveHealth(int amount);
    DamageResult RemoveHealthPercentage(float percentage);

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
}

public enum DamageResult
{
    Hit = 0,
    Missed = 1,
    Avoided = 2,
    Negated = 3,
    Immune = 4
}