using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Text;

public static class XmlUtil {
	#region Conversions
	public static string BoolToString(bool val)
	{
		return val.ToString();
	}
	public static bool StringToBool(string val)
	{
		bool b;
		if (bool.TryParse(val, out b)) return b;
		return false;
	}

	public static string FloatToString(float val, int precision = 3)
	{
		return string.Format("{0:F" + precision + "}", val);
	}

	public static float StringToFloat(string val)
	{
		float f;
		if (float.TryParse(val, out f)) return f;
		return 0;
	}

	public static string IntToString(int num)
	{
		return num.ToString();
	}

	public static int StringToInt(string numString)
	{
		int i;
		if (int.TryParse(numString, out i)) return i;
		return 0;
	}

	public static string VectorToString(Vector3 vec, int precision = 3)
	{
		return string.Format("({0:F"+precision+"},{1:F"+precision+"},{2:F"+precision+"})", vec.x, vec.y, vec.z); ;
	}

	public static Vector3 StringToVector(string s)
	{
		var fields = s.Trim('(', ')');
		var values = fields.Split(',');
		return new Vector3
		{
			x = float.Parse(values[0]),
			y = float.Parse(values[1]),
			z = float.Parse(values[2])
		};
	}
	#endregion

	public static StreamReader EncodeReadStream(Stream stream)
	{
		return new StreamReader(stream, Encoding.UTF8);
	}
	
	public static StreamWriter EncodeWriteStream(Stream stream)
	{
		return new StreamWriter(stream, Encoding.UTF8);
	}

	/// <summary>
	/// Will return default(T) if the file doesn't exist or the file cannot be deserialized
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <returns></returns>
	public static T ReadXmlFromFile<T>(string path)
	{
		if (!File.Exists(path)) return default(T);

		var serializer = new XmlSerializer(typeof(T));
		using (var stream = new FileStream(path, FileMode.Open))
		{
			using (var streamReader = new StreamReader(stream, Encoding.UTF8))
			{
				return (T)serializer.Deserialize(streamReader);
			}
		}
	}
	public static T ReadXmlFromFile<T>(string path, Action<System.Exception> onFail)
	{
		try
		{
			return ReadXmlFromFile<T>(path);
		}
		catch (System.Exception e)
		{
			onFail(e);
			return default(T);
		}
	}
	public static T ReadXmlFromFileDeleteInvalid<T>(string path)
	{
		try
		{
			return ReadXmlFromFile<T>(path);
		}
		catch (System.Exception e)
		{
            Debug.LogWarning (e);
			if (File.Exists(path)) File.Delete(path);
			return default(T);
		}
	}

	public static void WriteXmlToFile<T>(string path, T xml)
	{
		var serializer = new XmlSerializer(typeof(T));
		using (var stream = new FileStream(path, FileMode.Create))
		{
			using(var streamWriter = new StreamWriter(stream, Encoding.UTF8))
			{
				serializer.Serialize(streamWriter, xml);
			}
		}
	}
	public static T GetXml<T>(this Stream stream, string path)
	{
		var ser = new XmlSerializer(typeof(T));
		return (T)ser.Deserialize(stream);
	}

	public static bool WriteXml<T>(this Stream stream, string path, T xml)
	{
		var ser = new XmlSerializer(typeof(T));
		ser.Serialize(stream, xml);
		return true;
	}
}