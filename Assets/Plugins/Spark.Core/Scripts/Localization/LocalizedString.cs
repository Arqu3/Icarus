using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spark.Localization
{
	/// <summary>
	/// The string must be on a MonoBehaviour
	/// </summary>
	[System.Serializable]
	public class LocalizedString
	{
		/// <summary>
		/// The unlocalized value of the string
		/// </summary>
		[TextArea(1, 4)]
		public string baseValue;

		public string value
		{
			get
			{
				return Localizer.Instance.GetString(id);
			}
		}

		[Tooltip("Not to be altered over time")]
		public string id;
		[Tooltip("A descriptive text")]
		public string description;

		public LocalizedString()
		{
			if (string.IsNullOrEmpty(id))
			{
				id = System.Guid.NewGuid().ToString();
			}
		}
		public LocalizedString(string description, string id)
		{
			this.description = description;
			this.id = id;
			if (string.IsNullOrEmpty(id))
			{
				id = System.Guid.NewGuid().ToString();
			}
		}
		public LocalizedString(string description)
		{
			this.description = description;
			if (string.IsNullOrEmpty(id))
			{
				id = System.Guid.NewGuid().ToString();
			}
		}

		public static explicit operator string(LocalizedString s)
		{
			return s.value;
		}

		public string WithFormat(params object[] args)
		{
			var str = value;
			if (string.IsNullOrEmpty(str)) return str;
			try
			{
				return string.Format(str, args);
			}
			catch (System.FormatException e)
			{
				Debug.LogWarning(e);
				return str;
			}
		}
	}
}

