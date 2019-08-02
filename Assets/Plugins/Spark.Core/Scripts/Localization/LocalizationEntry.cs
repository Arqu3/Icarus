using UnityEngine;
namespace Spark.Localization
{
	/// <summary>
	/// Represents a language option
	/// </summary>
	[CreateAssetMenu(fileName = "New Entry", menuName = "Localization Entry")]
	public class LocalizationEntry : ScriptableObject
	{
		public string languageName = "English";
		public string code = "en-gb";
		public TextAsset file;
		public Font replacementFont;
	}
}

