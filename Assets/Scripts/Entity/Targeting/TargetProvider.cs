using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public static List<ICombatEntity> GetHeroes() => (from h in entities where h.EntityType == EntityType.Friendly select h).ToList();
    public static List<ICombatEntity> GetEnemies() => (from h in entities where h.EntityType == EntityType.Enemy select h).ToList();
}
