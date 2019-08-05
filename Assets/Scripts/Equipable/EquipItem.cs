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

#if UNITY_EDITOR

    public void OutputUsedStats()
    {
        Debug.Log("----------ITEM-----------");
        foreach(var m in mods)
        {
            foreach(var s in m.GetUsedStats())
            {
                Debug.Log(s.type + "\n" + s.value + "\n" + s.mathType);
            }
        }
        Debug.Log("====END ITEM====");
    }

#endif
}
