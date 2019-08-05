using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventory : BaseInventory
{
    [SerializeField]
    List<EquipableItem> items = new List<EquipableItem>();

    public override InventoryInteraction Give(EquipableItem item, out EquipableItem result)
    {
        result = null;
        items.Add(item);
        return InventoryInteraction.Give;
    }

    public override InventoryInteraction Take(EquipableItem item, out EquipableItem result)
    {
        result = item;
        items.Remove(item);
        return InventoryInteraction.Take;
    }
}
