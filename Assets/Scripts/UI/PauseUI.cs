using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class PauseUI : InstantiatableUI<PauseUI, PauseCanvas>
{
	public readonly UnityEvent onResume = new UnityEvent();
	public readonly UnityEvent onSettingsOpen = new UnityEvent();
	public readonly UnityEvent onExitMenu = new UnityEvent();
	public readonly UnityEvent onExitGame = new UnityEvent();

	public PauseUI(UnityAction<PauseUI> configure = null) : base(configure)
	{
		HookSelectableEvent(Canvas.resumeButton, onResume);
		HookSelectableEvent(Canvas.settingsButton, onSettingsOpen);
		HookSelectableEvent(Canvas.exitToMenuButton, onExitMenu);
		HookSelectableEvent(Canvas.exitGameButton, onExitGame);
	}
}