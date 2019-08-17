using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero //: IHero
{
    string heroName;
    public int Level { get; private set; } = 1;

    public Hero(HeroEntity prefab)
    {
        Prefab = prefab;
        heroName = NameGenerator.GetRandom();
    }

    public const int ITEMSLOTS = 3;

    public HeroState state = HeroState.Applying;
    public List<EquipableItem> Items = new List<EquipableItem>();
    public HeroEntity Prefab { get; private set; }

    public string GetDescription() => Prefab.GetDescription(heroName, Level);
    public void LevelUp()
    {
        ++Level;
    }
}

public enum HeroState
{
    Applying = 0,
    Recruited = 1,
    Selected = 2
}