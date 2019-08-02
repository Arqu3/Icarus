using UnityEngine;
using UnityEngine.UI;

namespace Spark.Localization
{
	/// <summary>
	/// Modifies the Text Component so it displays the localized text using the localized font
	/// </summary>
	[RequireComponent(typeof(Text))]
	[DefaultExecutionOrder(-10000)]
	[AddComponentMenu("Spark UI/Localized Text")]
	public class LocalizedText : MonoBehaviour
	{
		public string id;

		public string description;

		private Font srcFont;

		void OnValidate()
		{
			if (string.IsNullOrEmpty(id))
			{
				id = System.Guid.NewGuid().ToString();
			}
		}

		private void OnEnable()
		{
			srcFont = GetComponent<Text>().font;
			UpdateText();
			Localizer.Instance.onLanguageChanged.AddListener(UpdateText);
		}

		private void OnDisable()
		{
			Localizer.Instance.onLanguageChanged.RemoveListener(UpdateText);
		}

		private void UpdateText()
		{
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

		public void SetId(string id)
		{
			this.id = id;
			UpdateText();
		}
	}
}

