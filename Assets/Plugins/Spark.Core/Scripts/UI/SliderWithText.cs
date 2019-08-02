using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Spark.UI
{
	[RequireComponent(typeof(Slider))]
	public class SliderWithText : MonoBehaviour
	{

		[SerializeField]
		private ReplaceableText text;
		// Use this for initialization
		void Start()
		{
			var slider = GetComponent<Slider>();
			text.SetFormat(slider.value);
			slider.onValueChanged.AddListener(f =>
			{
				text.SetFormat(f);
			});
		}
		
	}

}
