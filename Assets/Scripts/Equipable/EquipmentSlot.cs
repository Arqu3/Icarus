using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot
{
    EntityModifier modifier;
    IInventory inventory;
    IItem current;

    public EquipmentSlot(EntityModifier modifier, IInventory inventory)
    {
        this.inventory = inventory;
        this.modifier = modifier;
    }

    public IItem Equip(IItem item)
    {
        if (current != null) UnEquip(current);

        current = item;
        modifier.ApplyItem(current as EquipItem);

        return current;
    }

    public IItem UnEquip(IItem item)
    {
        inventory.Give(current, out IItem result);
        current = null;
        return item;
    }

    public EquipItem Current => current == null ? null : current as EquipItem;
}