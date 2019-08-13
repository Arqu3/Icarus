using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class EmbarkUI : InstantiatableUI<EmbarkUI, EmbarkCanvas>
{
    public readonly UnityEvent OnStart = new UnityEvent();
    public readonly UnityEvent OnBack = new UnityEvent();

	public EmbarkUI(HeroUI heroUI, UnityAction<EmbarkUI> configure = null ) : base( configure )
	{
        Canvas.back.onClick.AddListener(() =>
        {
            foreach (var h in HeroCollection.Instance.GetSelected()) h.state = HeroState.Recruited;
            heroUI.UpdateList();
            Canvas.ChangeRoster();
        });

        heroUI.OnHeroSelected.AddListener((h) =>
        {
            if (!IsShowing) return;

            if (Canvas.AddToRoster(h)) heroUI.UpdateList();
        });

        HookSelectableEvent(Canvas.start, OnStart);
        HookSelectableEvent(Canvas.back, OnBack);
	}
}