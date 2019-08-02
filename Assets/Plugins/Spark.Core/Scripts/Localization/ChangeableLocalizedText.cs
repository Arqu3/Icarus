using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Spark.Localization
{
	[RequireComponent(typeof(Text))]
	[DefaultExecutionOrder(-10000)]
	[AddComponentMenu("Spark UI/Changeable Localized Text")]
	public class ChangeableLocalizedText : MonoBehaviour
	{
		[ShowInInspector]
		private string id;

		private Font srcFont;

		private object[] additionalParams;

		[ShowInInspector, ReadOnly]
		private bool formatText;

		private void OnValidate()
		{
			formatText = GetComponent<Text>().text.Contains("{0");
		}

		private void Awake()
		{
			srcFont = srcFont == null ? GetComponent<Text>().font : srcFont;
			Localizer.Instance.onLanguageChanged.AddListener(UpdateText);
		}

		private void UpdateText()
		{

			if (formatText)
			{
				additionalParams[0] = Localizer.Instance.GetString(id);
				GetComponent<Text>().text = string.Format(GetComponent<Text>().text, additionalParams);
			}
			else
				GetComponent<Text>().text = Localizer.Instance.GetString(id);
			if (Localizer.Instance.currentEntry.replacementFont)
			{
				GetComponent<Text>().font = Localizer.Instance.currentEntry.replacementFont;
			}
			else
			{
				GetComponent<Text>().font = srcFont;
			}
		}

		public void SetId(string id, params object[] additionalParams)
		{
			srcFont = srcFont == null ? GetComponent<Text>().font : srcFont;
			this.id = id;
			UpdateText();
			this.additionalParams = new object[additionalParams.Length + 1];
			for (int i = 1; i < additionalParams.Length; i++)
			{
				this.additionalParams[i] = additionalParams[i];
			}
		}
	}
}

