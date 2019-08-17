using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class InventoryUI : InstantiatableUI<InventoryUI, InventoryCanvas>
{
	public InventoryUI( UnityAction<InventoryUI> configure = null ) : base( configure )
	{
        onHide.AddListener(() => ItemTooltipUI.Instance.Clear());
	}

    public EquipableItem Take(EquipableItem item)
    {
        return Canvas.Take(item);
    }

    public void Give(EquipableItem item)
    {
        Canvas.Give(item);
    }

    public void SetItem(EquipableItem item)
    {
        Canvas.SetItem(item);
    }
}