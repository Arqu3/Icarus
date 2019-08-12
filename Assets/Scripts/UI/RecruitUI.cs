using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class RecruitUI : InstantiatableUI<RecruitUI, RecruitCanvas>
{
    public readonly UnityEvent OnBack = new UnityEvent();

	public RecruitUI( UnityAction<RecruitUI> configure = null ) : base( configure )
	{
        HookSelectableEvent(Canvas.back, OnBack);
	}

    public override void Show()
    {
        Canvas.ClearButtons();
        base.Show();
        foreach (var h in HeroCollection.Instance.GetApplying()) Canvas.CreateButton(h);
    }

    public override void Hide()
    {
        Canvas.SetInspectorPanelState(false);
        base.Hide();
    }
}