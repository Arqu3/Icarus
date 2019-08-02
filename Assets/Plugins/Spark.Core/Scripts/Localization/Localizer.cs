using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
namespace Spark.Localization
{
	/// <summary>
	/// The database of all translations. Is able to load different languages at runtime
	/// </summary>
	[ResourceSingleton]
	public class Localizer : MonoSingleton<Localizer>
	{

		public readonly UnityEvent onLanguageChanged = new UnityEvent();

		[SerializeField]
		private LocalizationEntry[] localizations;

		[SerializeField]
		private string defaultLanguageCode = "en-gb";

		private Dictionary<string, string> translations = new Dictionary<string, string>();

		public string currentCode { get; private set; }
		public LocalizationEntry currentEntry { get { return localizations[currentIndex]; } }
		public int currentIndex { get; private set; }

		private void Awake()
		{
			if (localizations.Length == 0) return;
			Load(PlayerPrefs.GetString("language", defaultLanguageCode));
		}

		public LocalizationEntry[] GetEntries()
		{
			return localizations;
		}

		public void Load(string code)
		{
			currentCode = code;
			TextAsset s = null;
			for (int i = 0; i < localizations.Length && s == null; i++)
			{
				if (localizations[i].code.Equals(code))
				{
					s = localizations[i].file;
					currentIndex = i;
				}
			}
			Debug.Assert(s != null);
			translations.Clear();
			Parse(s);
			onLanguageChanged.Invoke();
		}

		public string GetString(string id)
		{
			if (translations.ContainsKey(id))
				return translations[id];
			return null;
		}

		private void Parse(TextAsset localization)
		{
			using (var stream = new Mono.Csv.CsvFileReader(new MemoryStream(localization.bytes)))
			{
				stream.Delimiter = ',';
				stream.Quote = '"';
				List<string> cols = new List<string>();
				while (stream.ReadRow(cols))
				{
					translations.Add(cols[0], cols[1]);
				}
			}
		}
	}
}