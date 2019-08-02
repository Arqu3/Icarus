using UnityEngine;
namespace Spark.Localization
{
	using Tooltip;

	[AddComponentMenu("Spark UI/Localized Tooltip Element")]
	[RequireComponent(typeof(RectTransform))]
	public class LocalizedTooltipElement : TooltipElement
	{
		public LocalizedString localizedText;

		public override string GetTooltipText()
		{
			return localizedText.value;
		}
	}
}

