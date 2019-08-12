using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroRepresentation : IHeroRepresentation
{
    public HeroRepresentation(HeroEntity prefab)
    {
        Prefab = prefab;
    }

    public HeroRepState repState = HeroRepState.Applying;
    List<EquipableItem> items = new List<EquipableItem>();
    public HeroEntity Prefab { get; private set; }
    public List<EquipableItem> Items => items;
}

public enum HeroRepState
{
    Applying = 0,
    Recruited = 1,
    Selected = 2
}