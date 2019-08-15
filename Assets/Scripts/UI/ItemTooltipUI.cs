using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class ItemTooltipUI : InstantiatableUI<ItemTooltipUI, ItemTooltipCanvas>
{
    private static ItemTooltipUI _Instance;
    public static ItemTooltipUI Instance => _Instance ?? (_Instance = new ItemTooltipUI());

    private ItemTooltipUI(UnityAction<ItemTooltipUI> configure = null ) : base( configure )
	{
	}

    public void SetItem(EquipableItem item, Vector2 screenpos)
    {
        Canvas.SetItem(item, screenpos);
    }

    public void Clear()
    {
        Canvas.Clear();
    }
}