using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero : IHero
{
    public Hero(HeroEntity prefab)
    {
        Prefab = prefab;
    }

    public HeroState state = HeroState.Applying;
    List<EquipableItem> items = new List<EquipableItem>();
    public HeroEntity Prefab { get; private set; }
    public List<EquipableItem> Items => items;

    public string GetDescription() => Prefab.GetDescription();
}

public enum HeroState
{
    Applying = 0,
    Recruited = 1,
    Selected = 2
}