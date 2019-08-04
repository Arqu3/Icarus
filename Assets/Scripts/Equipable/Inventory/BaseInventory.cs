using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory : IInventory
{
    public abstract InventoryInteraction Give(IItem item, out IItem result);
    public abstract InventoryInteraction Take(IItem item, out IItem result);
}
