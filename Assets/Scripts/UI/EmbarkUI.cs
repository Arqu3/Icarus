using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class EmbarkUI : InstantiatableUI<EmbarkUI, EmbarkCanvas>
{
    public readonly UnityEvent OnStart = new UnityEvent();
    public readonly UnityEvent OnBack = new UnityEvent();

	public EmbarkUI(HeroUI heroUI, HeroInspectUI inspectUI , UnityAction<EmbarkUI> configure = null ) : base( configure )
	{
        Canvas.back.onClick.AddListener(() =>
        {
            foreach (var h in HeroCollection.Instance.GetSelected()) h.state = HeroState.Recruited;
            heroUI.UpdateList();
            Canvas.ChangeRoster();
            inspectUI.Hide();
        });

        heroUI.OnHeroSelected.AddListener((h) =>
        {
            if (!IsShowing) return;

            if (Canvas.AddToRoster(h)) heroUI.UpdateList();
            if (h == inspectUI.CurrentHero) inspectUI.Hide();
        });

        Canvas.OnHeroRemovedFromRoster.AddListener((h) =>
        {
            heroUI.UpdateList();
            if (h == inspectUI.CurrentHero) inspectUI.Hide();
        });

        HeroUIHelper.SetupInspectEvent(inspectUI, Canvas.OnHeroInspected);
        //Canvas.OnHeroInspected.AddListener((h) => inspectUI.ShowAndSet(h));

        HookSelectableEvent(Canvas.start, OnStart);
        HookSelectableEvent(Canvas.back, OnBack);
	}
}