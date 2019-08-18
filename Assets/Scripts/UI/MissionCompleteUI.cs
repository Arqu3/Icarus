using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class MissionCompleteUI : InstantiatableUI<MissionCompleteUI, MissionCompleteCanvas>
{
	public MissionCompleteUI(MissionLoot loot, UnityAction<MissionCompleteUI> configure = null ) : base( configure )
	{
        if (loot != null) Canvas.Victory(loot);
        else Canvas.Defeat();
	}
}