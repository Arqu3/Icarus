using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatEntity
{
    int Health { get; }
    float HealthPercentage { get; }
    void GiveHealth(int amount);
    void RemoveHealth(int amount);
    void RemoveHealthPercentage(float percentage);

    float Resource { get; }
    float ResourcePercentage { get; }
    bool SpendResource(float amount);
    bool SpendResourcePercentage(float percentage);
    void GiveResource(float amount);

    EntityType EntityType { get; }

    bool Valid { get; }

    GameObject gameObject { get; }
    Transform transform { get; }

    Coroutine StartCoroutine(IEnumerator ienumerator);
}
