using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
namespace Spark.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	[AddComponentMenu("Spark UI/Replaceable Text")]
	public class ReplaceableText : MonoBehaviour
	{
		public string textTemplate
		{
			get
			{
				return template;
			}
		}
		public string text
		{
			get
			{
				return textComponent ? textComponent.text : GetComponent<TextMeshProUGUI>().text;
			}
			set
			{
				(textComponent ? textComponent : GetComponent<TextMeshProUGUI>()).text = value;
			}
		}
		public new bool enabled
		{
			get
			{
				return textComponent ? textComponent.enabled : GetComponent<TextMeshProUGUI>().enabled;
			}
			set
			{
				if (textComponent)
					textComponent.enabled = value;
				else GetComponent<TextMeshProUGUI>().enabled = value;
			}
		}
		public TextMeshProUGUI textComponent
		{
			get;
			private set;
		}
		[ShowInInspector]
		private string template;


		void Awake()
		{
			textComponent = GetComponent<TextMeshProUGUI>();
			template = textComponent.text;
		}
		/// <summary>
		/// Set text value using string.Format with the template as the string to format. Arguments must match arguments in the format string
		/// </summary>
		/// <param name="args"></param>
		public void SetFormat(params object[] args)
		{
			if (textTemplate == null) return;
			try
			{
				text = string.Format(textTemplate, args);
			}
			catch (System.Exception e)
			{
				Debug.LogWarning("Could not format string '" + text + "' with " + args.Length + " arguments ");
				Debug.LogWarning(e);
			}
		}
	}
}
