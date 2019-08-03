using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ItemModData), menuName = "Item Data/" + nameof(ItemModData))]
public class ItemModData : ItemResourceData<ItemModData>
{
    public List<ItemMod> mods = new List<ItemMod>();
}

public enum ModType
{
    Prefix = 0,
    Suffix = 1
}

public enum ModMathType
{
    Additive = 0,
    Multiplicative = 1
}

[System.Serializable]
public class ModMath
{
    public ModMathType mathType = ModMathType.Additive;
}

[System.Serializable]
public class PowerMod : ModMath
{
    public int value;
}

[System.Serializable]
public struct ItemMod
{
    [Header("Debug")]
    [SerializeField]
    bool debug;

    [Header("Name, type")]
    public string name;
    public ModType modType;

    [Header("Stats")]
    public PowerMod power;
    public float resourceGain;
    public float actionCooldown;
}