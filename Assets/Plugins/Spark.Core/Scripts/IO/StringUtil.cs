using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
namespace Spark.IO
{
	public static class StringExtensionUtils
	{
		/// <summary>
		/// Includes extension
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string GetFileOrFolderName(this string path)
		{

			return Regex.Match(path, @"[^\\/]+[\\/]?$").Value;
		}

		/// <summary>
		/// for files named 'file.xml.project.cache' will remove the outer extension signifier resulting in 'file.xml.project'
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string RemoveOuterExtension(this string path)
		{
			return path.Substring(0, path.LastIndexOf('.'));
		}

		/// <summary>
		/// for files named 'file.xml.project.cache' will remove all extension signifiers resulting in 'file'
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string RemoveFullExtension(this string path)
		{
			string s = path;
			var arr = new[] { '\\', '/' };
			while (s.LastIndexOfAny(arr) < s.LastIndexOf('.'))
			{
				s = s.Substring(0, s.LastIndexOf('.'));
			}
			return s;
		}
	}
}

