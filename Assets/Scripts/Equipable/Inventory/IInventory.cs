using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    InventoryInteraction Give(IItem item, out IItem result);
    InventoryInteraction Take(IItem item, out IItem result);
}
