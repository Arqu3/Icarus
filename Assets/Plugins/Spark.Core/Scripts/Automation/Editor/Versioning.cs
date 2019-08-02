using UnityEditor;

namespace Spark.Automation
{
	public class Versioning
	{
		[MenuItem("Tools/Bump Version/Current")]
		public static void ShowCurrent()
		{
			EditorUtility.DisplayDialog("Current version", PlayerSettings.bundleVersion, "ok");
		}
		[MenuItem("Tools/Bump Version/Major")]
		public static void BumpMajorVersion()
		{
			PlayerSettings.bundleVersion = UpVersionString(PlayerSettings.bundleVersion, true, false, false, false);
			AssetDatabase.SaveAssets();
		}
		[MenuItem("Tools/Bump Version/Minor")]
		public static void BumpMinorVersion()
		{
			PlayerSettings.bundleVersion = UpVersionString(PlayerSettings.bundleVersion, false, true, false, false);
			AssetDatabase.SaveAssets();
		}
		[MenuItem("Tools/Bump Version/Patch")]
		public static void BumpPatchVersion()
		{
			PlayerSettings.bundleVersion = UpVersionString(PlayerSettings.bundleVersion, false, false, true, false);
			AssetDatabase.SaveAssets();
		}
		[MenuItem("Tools/Bump Version/Internal")]
		public static void BumpInternalVersion()
		{
			PlayerSettings.bundleVersion = UpVersionString(PlayerSettings.bundleVersion, false, false, false, true);
			AssetDatabase.SaveAssets();
		}

		public static string UpVersionString(string versionString, bool major, bool minor, bool patch, bool intern)
		{
			var f = versionString.Split('f');
			var split = f[0].Split('.');
			switch (split.Length)
			{
				case 0:
					return string.Format("{0}.{1}.{2}f{3}", ConditionalParse(split[0], major, false), ConditionalParse("0", minor, major), ConditionalParse("0", patch, major || minor), ConditionalParse(f.Length > 1 ? f[1] : "0", intern, major || minor || patch));
				case 1:
					return string.Format("{0}.{1}.{2}f{3}", ConditionalParse(split[0], major, false), ConditionalParse(split[1], minor, major), ConditionalParse("0", patch, major || minor), ConditionalParse(f.Length > 1 ? f[1] : "0", intern, major || minor || patch));
				case 2:
					return string.Format("{0}.{1}.{2}f{3}", ConditionalParse(split[0], major, false), ConditionalParse("0", minor, major), ConditionalParse("0", patch, major || minor), ConditionalParse(f.Length > 1 ? f[1] : "0", intern, major || minor || patch));
				default:
					return string.Format("{0}.{1}.{2}f{3}", ConditionalParse(split[0], major, false), ConditionalParse(split[1], minor, major), ConditionalParse(split[2], patch, major || minor), ConditionalParse(f.Length > 1 ? f[1] : "0", intern, major || minor || patch));
			}
		}

		private static string ConditionalParse(string str, bool increment, bool reset)
		{
			return reset ? "0" : increment ? (int.Parse(str) + 1).ToString() : str;
		}
	}
}

