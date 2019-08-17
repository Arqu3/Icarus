using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot
{
    EntityModifier modifier;
    //IInventory inventory;
    EquipableItem current;

    public EquipmentSlot(EntityModifier modifier)//, IInventory inventory)
    {
        //this.inventory = inventory;
        this.modifier = modifier;
    }

    public EquipableItem Equip(EquipableItem item)
    {
        if (current != null) UnEquip(item);

        current = item;
        modifier.ApplyItem(current);

        return current;
    }

    public EquipableItem UnEquip(EquipableItem item)
    {
        //inventory.Give(current, out EquipableItem result);
        modifier.RemoveItem(current);
        current = null;
        return item;
    }

    public EquipableItem Current => current ?? null;
}