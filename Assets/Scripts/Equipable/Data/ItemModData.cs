using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ItemModData), menuName = "Item Data/" + nameof(ItemModData))]
public class ItemModData : ItemResourceData<ItemModData>
{
    public List<ItemMod> mods = new List<ItemMod>();
    public List<bool> showBools = new List<bool>();

    public const float UseThreshold = 0.0001f;
}