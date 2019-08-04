using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemCreator
{
    public static EquipItem CreateRandomItem()
    {
        var baseItem = new EquipItem();
        int prefixNum = Random.Range(0, 4);
        int suffixNum = Random.Range(0, 4);

        if (prefixNum + suffixNum == 0)
        {
            if (Random.Range(0f, 1f) > 0.5) prefixNum++;
            else suffixNum++;
        }

        var data = ItemModData.Instance;
        List<ItemMod> selectedMods = new List<ItemMod>();
        List<ItemMod> availableMods = new List<ItemMod>();
        foreach (var m in data.mods) availableMods.Add(m);

        for(int i = 0; i < prefixNum; ++i)
        {
            SelectMod(availableMods, selectedMods, ModType.Prefix);
        }

        for (int i = 0; i < suffixNum; ++i)
        {
            SelectMod(availableMods, selectedMods, ModType.Suffix);
        }

        foreach (var selected in selectedMods) baseItem.mods.Add(selected);

        return baseItem;
    }

    static void SelectMod(List<ItemMod> availableMods, List<ItemMod> selectedMods, ModType type)
    {
        var mods = (from m in availableMods where m.modType == type select m).ToArray();
        if (mods.Count() > 0)
        {
            var selectedMod = mods.Random();
            selectedMods.Add(selectedMod);
            availableMods.Remove(selectedMod);
        }
    }
}
