using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EquipableItem
{
    public ItemRarity rarity = ItemRarity.Common;

    public List<ItemMod> mods = new List<ItemMod>();

    public List<IStatDecorator> statDecorators = new List<IStatDecorator>();
    public List<IHealthDecorator> healthDecorators = new List<IHealthDecorator>();

    public StatStruct[] GetUsedStats() => mods.SelectMany(x => x.GetUsedStats()).ToArray();
}
