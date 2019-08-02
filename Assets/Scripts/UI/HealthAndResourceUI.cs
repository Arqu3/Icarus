using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class HealthAndResourceUI : InstantiatableUI<HealthAndResourceUI, HealthAndResourceCanvas>
{

	public HealthAndResourceUI( UnityAction<HealthAndResourceUI> configure = null ) : base( configure )
	{
		
	}
}