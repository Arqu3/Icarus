using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class HeroInspectUI : InstantiatableUI<HeroInspectUI, HeroInspectCanvas>
{
    public readonly UnityEvent OnClose = new UnityEvent();
    public Hero CurrentHero { get; private set; }

	public HeroInspectUI(UnityAction<HeroInspectUI> configure = null ) : base( configure )
	{
        Canvas.closeButton.onClick.AddListener(Hide);
        HookSelectableEvent(Canvas.closeButton, OnClose);	
	}

    public void ShowAndSet(Hero hero)
    {
        Show();
        Canvas.ShowHero(hero);
        CurrentHero = hero;
    }

    public override void Hide()
    {
        CurrentHero = null;
        base.Hide();
    }
}