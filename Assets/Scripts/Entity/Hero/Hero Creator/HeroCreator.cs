using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HeroCreator
{
    public static HeroEntity CreateLiveFrom(IHero hero, Vector3 position)
    {
        var inst = Object.Instantiate(hero.Prefab, position, hero.Prefab.transform.rotation);
        for (int i = 0; i < Mathf.Min(hero.Items.Count, inst.EquipmentSlots.Length); ++i)
        {
            if (hero.Items[i] != null) inst.EquipmentSlots[i].Equip(hero.Items[i]);
        }
        return inst;
    }

    public static Hero CreateRandomHero()
    {
        return new Hero(EntityPrefabs.Instance.heroes.Random());
    }
}
