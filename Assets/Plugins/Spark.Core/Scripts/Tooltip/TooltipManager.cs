using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Tooltip
{
	public class TooltipManager : MonoSingleton<TooltipManager>
	{

		private TooltipUI tooltipUi;
		void Awake()
		{
			tooltipUi = new TooltipUI();
			tooltipUi.Show();
		}

		public void SetTooltip(string tooltip, Vector3 screenPos, bool flip = false)
		{
			tooltipUi.SetTooltip(tooltip, screenPos, flip);
		}

		public void UpdateText(string tooltip)
		{
			tooltipUi.UpdateText(tooltip);
		}

		public void ClearTooltip()
		{
			tooltipUi.ClearTooltip();
		}
	}
}

