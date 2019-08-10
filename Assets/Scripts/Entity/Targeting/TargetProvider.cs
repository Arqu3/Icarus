using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetProvider
{
    static List<ICombatEntity> entities = new List<ICombatEntity>();

    public static void Add(ICombatEntity entity)
    {
        entities.Add(entity);
    }

    public static void Remove(ICombatEntity entity)
    {
        entities.Remove(entity);
    }

    public static List<ICombatEntity> Get() => entities;
}
