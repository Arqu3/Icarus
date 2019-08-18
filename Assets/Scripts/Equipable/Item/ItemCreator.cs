using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemCreator
{
    public static EquipableItem CreateRandomItem()
    {
        var baseItem = new EquipableItem();
        baseItem.rarity = GetItemRarity();
        int num = (int)baseItem.rarity;
        int prefixNum = num + 1;//Random.Range(num, num + 2);
        int suffixNum = num + 1;//Random.Range(num, num + 2);

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
            SelectMod(availableMods, selectedMods, ModType.Prefix, baseItem.rarity);
        }

        for (int i = 0; i < suffixNum; ++i)
        {
            SelectMod(availableMods, selectedMods, ModType.Suffix, baseItem.rarity);
        }

        foreach (var selected in selectedMods) baseItem.mods.Add(selected);

        return baseItem;
    }

    static void SelectMod(List<ItemMod> availableMods, List<ItemMod> selectedMods, ModType type, ItemRarity rarity)
    {
        var mods = GetRarityMods((from m in availableMods where m.modType == type select m).ToArray(), rarity);
        if (mods.Count() > 0)
        {
            var selectedMod = mods.Random();

            var selectedUsed = selectedMod.GetUsedStats().FirstOrDefault();
            //Does not take hybrid mods into consideration
            var same = (from m
                        in selectedMods
                        let used = m.GetUsedStats().FirstOrDefault()
                        where used.mathType == selectedUsed.mathType && used.type == selectedUsed.type
                        select m).ToArray();
            bool reroll = same.Length > 0;

            if (reroll) selectedMod = mods.Random();

            selectedMods.Add(selectedMod);
            availableMods.Remove(selectedMod);
        }
    }

    static ItemRarity GetItemRarity()
    {
        var list = new List<ItemRarity> { ItemRarity.Legendary, ItemRarity.Rare, ItemRarity.Common };
        return GetWeightedEntry(list, 0.1f, 0.4f, 1f);
    }

    static ItemMod[] GetRarityMods(ItemMod[] available, ItemRarity rarity)
    {
        int tier = GetModTier(rarity);
        return (from m in available where m.tier == tier select m).ToArray();
    }

    static int GetModTier(ItemRarity rarity)
    {
        List<int> tiers = new List<int>() { 3, 2, 1 };

        switch (rarity)
        {
            case ItemRarity.Common:
                return GetWeightedEntry(tiers, 0.7f, 0.4f, 0.1f);
            case ItemRarity.Rare:
                return GetWeightedEntry(tiers, 0.3f, 0.7f, 0.15f);
            case ItemRarity.Legendary:
                return GetWeightedEntry(tiers, 0.2f, 0.45f, 1f);
            default:
                break;
        }

        return tiers.First();
    }

    static T GetWeightedEntry<T>(List<T> list, params float[] weights)
    {
        float chance = Random.Range(0f, 1f);
        for(int i = 0; i < list.Count; ++i)
        {
            int wi = Mathf.Min(i, weights.Length - 1);
            if (chance <= Mathf.Clamp01(Mathf.Abs(weights[wi]))) return list[i];
        }

        return list.LastOrDefault();
    }
}
