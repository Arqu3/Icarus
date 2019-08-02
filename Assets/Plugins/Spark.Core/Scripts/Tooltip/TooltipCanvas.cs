using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;

namespace Spark.Tooltip
{
	[AddComponentMenu("")]
	public class TooltipCanvas : InstantiatableCanvas
	{
		public Text tooltipText;
		public RectTransform tooltipBox;

		private void Start()
		{
			tooltipText.text = "";
			foreach (var v in tooltipBox.GetComponentsInChildren<Graphic>())
			{
				v.CrossFadeAlpha(0f, 0f, true);
			}
			DontDestroyOnLoad(gameObject);
		}
	}
}