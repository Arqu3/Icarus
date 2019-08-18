using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class PreCombatUI : InstantiatableUI<PreCombatUI, PreCombatCanvas>
{
    public readonly UnityEvent OnStart = new UnityEvent();
	public PreCombatUI( UnityAction<PreCombatUI> configure = null ) : base( configure )
	{
        HookSelectableEvent(Canvas.startButton, OnStart);
	}
}