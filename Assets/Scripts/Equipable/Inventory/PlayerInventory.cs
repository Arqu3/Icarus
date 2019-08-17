using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerInventory : BaseInventory
//{
//    List<EquipableItem> items = new List<EquipableItem>();

//    InventoryUI ui;

//    public PlayerInventory(out InventoryUI ui)
//    {
//        ui = new InventoryUI();
//        ui.Show();
//        this.ui = ui;
//    }

//    public override InventoryInteraction Give(EquipableItem item, out EquipableItem result)
//    {
//        result = null;
//        items.Add(item);
//        ui.Give(item);
//        return InventoryInteraction.Give;
//    }

//    public override InventoryInteraction Take(EquipableItem item, out EquipableItem result)
//    {
//        result = item;
//        items.Remove(item);
//        ui.Take(item);
//        return InventoryInteraction.Take;
//    }
//}