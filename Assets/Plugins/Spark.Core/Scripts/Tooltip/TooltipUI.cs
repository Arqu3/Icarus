using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Spark.UI;

namespace Spark.Tooltip
{
	public class TooltipUI : InstantiatableUI<TooltipUI, TooltipCanvas>
	{

		public float fadeTime = 0.25f;

		public TooltipUI(UnityAction<TooltipUI> configure = null) : base(configure)
		{

		}

		public void SetTooltip(string tooltip, Vector3 screenpos, bool flip)
		{
			Canvas.tooltipText.text = tooltip;
			var corners = new Vector3[4];
			Canvas.tooltipBox.GetWorldCorners(corners);
			Canvas.tooltipBox.position = flip ? screenpos - Vector3.right * (corners[3].x - corners[0].x) : screenpos;
			if (!string.IsNullOrEmpty(Canvas.tooltipText.text))
			{
				foreach (var v in Canvas.tooltipBox.GetComponentsInChildren<Graphic>())
				{
					v.CrossFadeAlpha(1f, fadeTime, true);
				}
			}
		}
		public void UpdateText(string tooltip)
		{
			Canvas.tooltipText.text = tooltip;
		}

		public void ClearTooltip()
		{
			Canvas.tooltipText.text = "";
			foreach (var v in Canvas.tooltipBox.GetComponentsInChildren<Graphic>())
			{
				v.CrossFadeAlpha(0f, fadeTime, true);
			}
		}
	}
}
