using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInventory : BaseInventory
{
    [SerializeField]
    List<IItem> items = new List<IItem>();

    public override InventoryInteraction Give(IItem item, out IItem result)
    {
        result = item;
        items.Add(item);
        return InventoryInteraction.Give;
    }

    public override InventoryInteraction Take(IItem item, out IItem result)
    {
        result = item;
        items.Remove(item);
        return InventoryInteraction.Take;
    }
}
