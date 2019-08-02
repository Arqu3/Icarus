using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Spark.Tooltip
{
	[AddComponentMenu("Tooltip/Text Tooltip")]
	public class TextTooltipElement : TooltipElement
	{

		[SerializeField]
		[Multiline]
		private string tooltipText = "Default Tooltip";

		private string tooltipTemplate;

		public virtual string TooltipText
		{
			get
			{
				return tooltipText;
			}
			set
			{
				tooltipText = value;
				RefreshIfShowing();
			}
		}

		public override string GetTooltipText()
		{
			return TooltipText;
		}

		void Awake()
		{
			tooltipTemplate = tooltipText;
		}

		public void SetTextFormat(params object[] args)
		{
			TooltipText = string.Format(tooltipText, args);
		}
	}
}