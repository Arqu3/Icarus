using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class InventoryUI : InstantiatableUI<InventoryUI, InventoryCanvas>
{

	public InventoryUI( UnityAction<InventoryUI> configure = null ) : base( configure )
	{
		
	}
}