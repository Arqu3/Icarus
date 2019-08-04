using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EquipItem : IItem
{
    public Rarity rarity = Rarity.Common;

    public List<ItemMod> mods = new List<ItemMod>();

    public List<IStatDecorator> statDecorators = new List<IStatDecorator>();
    public List<IHealthDecorator> healthDecorators = new List<IHealthDecorator>();

    public ConvertedStat[] GetUsedStats() => mods.SelectMany(x => x.GetUsedStats()).ToArray();
}
