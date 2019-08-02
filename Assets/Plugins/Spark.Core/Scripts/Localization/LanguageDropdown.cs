using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace Spark.Localization
{
	/// <summary>
	/// Auto fills the dropdown with available languages
	/// </summary>
	[RequireComponent(typeof(Dropdown))][AddComponentMenu("Spark UI/Language Dropdown")]
	public class LanguageDropdown : MonoBehaviour
	{
		
		void Start()
		{
			var d = GetComponent<Dropdown>();
			d.ClearOptions();
			d.AddOptions(Localizer.Instance.GetEntries().Select(e => e.languageName).ToList());
			d.value = Localizer.Instance.currentIndex;
			d.RefreshShownValue();
			d.onValueChanged.AddListener(OptionChange);
		}

		private void OptionChange(int index)
		{
			var code = Localizer.Instance.GetEntries()[index].code;
			Localizer.Instance.Load(code);
			PlayerPrefs.SetString("language", code);
		}

	}
}

