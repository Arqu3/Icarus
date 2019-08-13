using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class HeroUI : InstantiatableUI<HeroUI, HeroCanvas>
{
    public GenericUnityEvent<Hero> OnHeroSelected => Canvas.OnHeroSelected;

	public HeroUI( UnityAction<HeroUI> configure = null ) : base( configure )
	{
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