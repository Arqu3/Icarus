using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Spark.Localization
{
	/// <summary>
	/// Changes the behaviour of the Text Component so it will always use the localized font
	/// </summary>
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("Spark UI/Localized Font")]
	public class LocalizedFont : MonoBehaviour
	{
		private Font baseFont;

		private void OnValidate()
		{
			Debug.Assert(!GetComponent<LocalizedText>(), "You should not have both LocalizedFont and LocalizedText component on the same object. LocalizedText does the same job as LocalizedFont.");
		}

		// Use this for initialization
		void OnEnable()
		{
			if (!baseFont) baseFont = GetComponent<Text>().font;
			UpdateFont();
			Localizer.Instance.onLanguageChanged.AddListener(UpdateFont);
		}

		private void OnDisable()
		{
			Localizer.Instance.onLanguageChanged.RemoveListener(UpdateFont);
		}

		private void UpdateFont()
		{
			if (Localizer.Instance.currentEntry.replacementFont)
				GetComponent<Text>().font = Localizer.Instance.currentEntry.replacementFont;
			else
				GetComponent<Text>().font = baseFont;
		}
	}
}

