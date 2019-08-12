using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HeroCreator
{
    public static HeroEntity CreateLiveFrom(IHeroRepresentation rep, Vector3 position)
    {
        var inst = Object.Instantiate(rep.Prefab, position, rep.Prefab.transform.rotation);
        for (int i = 0; i < Mathf.Min(rep.Items.Count, inst.EquipmentSlots.Length); ++i)
        {
            if (rep.Items[i] != null) inst.EquipmentSlots[i].Equip(rep.Items[i]);
        }
        return inst;
    }

    public static HeroRepresentation CreateRandomRepresentation()
    {
        return new HeroRepresentation(EntityPrefabs.Instance.heroes.Random());
    }
}
