using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class HeroUI : InstantiatableUI<HeroUI, HeroCanvas>
{
    public GenericUnityEvent<Hero> OnHeroSelected => Canvas.OnHeroSelected;

	public HeroUI(HeroInspectUI inspectUI, UnityAction<HeroUI> configure = null ) : base( configure )
	{
        HeroUIHelper.SetupInspectEvent(inspectUI, Canvas.OnHeroInspected);
        //Canvas.OnHeroInspected.AddListener((h) => inspectUI.ShowAndSet(h));
	}

    public void UpdateList()
    {
        Canvas.UpdateList();
    }

    public override void Show()
    {
        base.Show();
        Canvas.UpdateList();
    }
}