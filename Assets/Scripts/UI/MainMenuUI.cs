using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;
public class MainMenuUI : InstantiatableUI<MainMenuUI, MainMenuCanvas>
{
	public readonly UnityEvent onStartGame = new UnityEvent();
	public readonly UnityEvent onSettingsOpen = new UnityEvent();
	public readonly UnityEvent onExit = new UnityEvent();
	
	public MainMenuUI() : base(null)
	{
		Canvas.gameTitle.text = Application.productName;
		HookSelectableEvent(Canvas.startButton, onStartGame);
		HookSelectableEvent(Canvas.settingsButton, onSettingsOpen);
		HookSelectableEvent(Canvas.exitButton, onExit);
	}

	public void HideSettings()
	{
		Canvas.settingsButton.gameObject.SetActive(false);
	}
}
