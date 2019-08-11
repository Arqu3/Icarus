using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class HubUI : InstantiatableUI<HubUI, HubCanvas>
{
    public readonly UnityEvent OnEmbark = new UnityEvent();
    public readonly UnityEvent OnHeroes = new UnityEvent();
    public readonly UnityEvent OnApplications = new UnityEvent();
    public readonly UnityEvent OnExitToMenu = new UnityEvent();

	public HubUI( UnityAction<HubUI> configure = null ) : base( configure )
	{
        HookSelectableEvent(Canvas.embark, OnEmbark);
        HookSelectableEvent(Canvas.heroes, OnHeroes);
        HookSelectableEvent(Canvas.applications, OnApplications);
        HookSelectableEvent(Canvas.exitToMenu, OnExitToMenu);
	}
}