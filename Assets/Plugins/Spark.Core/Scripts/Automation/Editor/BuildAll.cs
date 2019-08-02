using UnityEditor;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
namespace Spark.Automation
{
	using IO;
	
	public class BuildAll
	{

		private struct BuildSetting
		{
			public string name;
			public string ending;
			public BuildTarget buildTarget;
		}

		private const string MenuItemWin64 = "Publish/Build Targets/Windows 64-Bit";
		private const string MenuItemWin32 = "Publish/Build Targets/Windows 32-Bit";
		private const string MenuItemOsx = "Publish/Build Targets/Mac OSX";
		private const string MenuItemLnx64 = "Publish/Build Targets/Linux 64-Bit";
		private const string MenuItemLnx32 = "Publish/Build Targets/Linux 32-Bit";

		[MenuItem(MenuItemWin64, true)]
		public static bool ToggleWin64Validate()
		{
			Menu.SetChecked(MenuItemWin64, EditorPrefs.GetBool(MenuItemWin64, true));
			return true;
		}
		[MenuItem(MenuItemWin64, priority = 0)]
		public static void ToggleWin64()
		{
			var b = !EditorPrefs.GetBool(MenuItemWin64, true);
			Menu.SetChecked(MenuItemWin64, b);
			EditorPrefs.SetBool(MenuItemWin64, b);
		}

		[MenuItem(MenuItemWin32, true)]
		public static bool ToggleWin32Validate()
		{
			Menu.SetChecked(MenuItemWin32, EditorPrefs.GetBool(MenuItemWin32, true));
			return true;
		}
		[MenuItem(MenuItemWin32, priority = 1)]
		public static void ToggleWin32()
		{
			var b = !EditorPrefs.GetBool(MenuItemWin32, true);
			Menu.SetChecked(MenuItemWin32, b);
			EditorPrefs.SetBool(MenuItemWin32, b);
		}

		[MenuItem(MenuItemOsx, true)]
		public static bool ToggleOsxValidate()
		{
			Menu.SetChecked(MenuItemOsx, EditorPrefs.GetBool(MenuItemOsx, true));
			return true;
		}
		[MenuItem(MenuItemOsx, priority = 2)]
		public static void ToggleOsx()
		{
			var b = !EditorPrefs.GetBool(MenuItemOsx, true);
			Menu.SetChecked(MenuItemOsx, b);
			EditorPrefs.SetBool(MenuItemOsx, b);
		}

		[MenuItem(MenuItemLnx64, true)]
		public static bool ToggleLnx64Validate()
		{
			Menu.SetChecked(MenuItemLnx64, EditorPrefs.GetBool(MenuItemLnx64, true));
			return true;
		}
		[MenuItem(MenuItemLnx64, priority = 3)]
		public static void ToggleLnx64()
		{
			var b = !EditorPrefs.GetBool(MenuItemLnx64, true);
			Menu.SetChecked(MenuItemLnx64, b);
			EditorPrefs.SetBool(MenuItemLnx64, b);
		}

		[MenuItem(MenuItemLnx32, true)]
		public static bool ToggleLnx32Validate()
		{
			Menu.SetChecked(MenuItemLnx32, EditorPrefs.GetBool(MenuItemLnx32, true));
			return true;
		}
		[MenuItem(MenuItemLnx32, priority = 4)]
		public static void ToggleLnx32()
		{
			var b = !EditorPrefs.GetBool(MenuItemLnx32, true);
			Menu.SetChecked(MenuItemLnx32, b);
			EditorPrefs.SetBool(MenuItemLnx32, b);
		}

		[MenuItem("Publish/[Development] Build For Steam", priority = 1)]
		public static void BuildForSteamDevelop()
		{
			Versioning.BumpInternalVersion();
			string path = UnityEngine.Application.dataPath + "/../Steam Building Folder/sdk/tools/ContentBuilder/content";
			ToggleSteamworks.Toggle(false);
			BuildAllExecutables(path, BuildOptions.Development);
            var vdf =
    Directory.GetFiles(UnityEngine.Application.dataPath + "/../Steam Building Folder/sdk/tools/ContentBuilder/scripts/", "*.vdf")
    .First(f =>
       Regex.IsMatch(
           f,
           UnityEngine.Application.dataPath + @"/../Steam Building Folder/sdk/tools/ContentBuilder/scripts/app_build_[\d]+.vdf")
     );
            File.WriteAllText(vdf, Regex.Replace(File.ReadAllText(vdf), @"""desc"" ""[^""]+""", @"""desc"" """ + PlayerSettings.bundleVersion + "-development\""));
		}
		[MenuItem("Publish/[Release] Build For Steam", priority = 2)]
		public static void BuildForSteamRelease()
		{
			Versioning.BumpInternalVersion();
			string path = UnityEngine.Application.dataPath + "/../Steam Building Folder/sdk/tools/ContentBuilder/content";
			ToggleSteamworks.Toggle(false);
			BuildAllExecutables(path, BuildOptions.None);
            var vdf =
Directory.GetFiles(UnityEngine.Application.dataPath + "/../Steam Building Folder/sdk/tools/ContentBuilder/scripts/", "*.vdf")
.First(f =>
Regex.IsMatch(
f,
UnityEngine.Application.dataPath + @"/../Steam Building Folder/sdk/tools/ContentBuilder/scripts/app_build_[\d]+.vdf")
);
            File.WriteAllText(vdf, Regex.Replace(File.ReadAllText(vdf), @"""desc"" ""[^""]+""", @"""desc"" """ + PlayerSettings.bundleVersion + "-release\""));
		}


		[MenuItem("Publish/[Development] Build For All Platforms", priority = 3)]
		public static void BuildForAllDevelop()
		{
			Versioning.BumpInternalVersion();
			string path = EditorUtility.SaveFolderPanel("Choose Build root path", "", "");
			if (path == null || path.Equals("")) return;
			if (ToggleSteamworks.IsSteamworksDisabled())
			{
				BuildAllExecutables(path, BuildOptions.Development);
			}
			else
			{
				BuildAllExecutables(path, BuildOptions.Development, "DISABLESTEAMWORKS");
			}
		}

		[MenuItem("Publish/[Release] Build For All Platforms", priority = 4)]
		public static void BuildForAllRelease()
		{
			Versioning.BumpInternalVersion();
			string path = EditorUtility.SaveFolderPanel("Choose Build root path", "", "");
			if (path == null || path.Equals("")) return;
			if (ToggleSteamworks.IsSteamworksDisabled())
			{
				BuildAllExecutables(path, BuildOptions.None);
			}
			else
			{
				BuildAllExecutables(path, BuildOptions.None, "DISABLESTEAMWORKS");
			}
		}

		private static void BuildAllExecutables(string path, BuildOptions buildOptions, params string[] compileFlags)
		{
			string buildName = PlayerSettings.productName;
			Queue<Thread> threads = new Queue<Thread>();
			var levels = (from s in EditorBuildSettings.scenes
						  where s.enabled
						  select s.path).ToArray();

			var builds = (from setting in new[] {
				new { use = EditorPrefs.GetBool(MenuItemWin32, true), set = new BuildSetting { name =  "windows_32", ending = buildName+".exe", buildTarget = BuildTarget.StandaloneWindows} },
				new { use = EditorPrefs.GetBool(MenuItemOsx, true), set = new BuildSetting { name =  "osx", ending = buildName+".app", buildTarget = BuildTarget.StandaloneOSX } },
				new { use = EditorPrefs.GetBool(MenuItemLnx64, true), set = new BuildSetting { name =  "lnx64", ending = buildName+".x86_64", buildTarget = BuildTarget.StandaloneLinux64 } },
				new { use = EditorPrefs.GetBool(MenuItemLnx32, true), set = new BuildSetting { name =  "lnx32", ending = buildName+".x86", buildTarget = BuildTarget.StandaloneLinux } },
				new { use = EditorPrefs.GetBool(MenuItemWin64, true), set = new BuildSetting { name =  "windows_64", ending = buildName+".exe", buildTarget = BuildTarget.StandaloneWindows64 } } }
						  where setting.use
						  select setting.set).ToArray();

			var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, string.Join(";", PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';').Concat(compileFlags).Distinct().ToArray()));
			EditorUserBuildSettings.SetPlatformSettings("Standalone", "CopyPDBFiles", "enabled");
			foreach (var build in builds)
			{
				string savePath = path + "/" + build.name;
				BuildPipeline.BuildPlayer(levels, savePath + "/" + build.ending, build.buildTarget, buildOptions);

				if (build.buildTarget == BuildTarget.StandaloneWindows || build.buildTarget == BuildTarget.StandaloneWindows64)
				{
					foreach (var pdb in Directory.GetFiles(savePath, "*.pdb"))
					{
						Directory.CreateDirectory(savePath + "/../pdb/");
						File.Copy(pdb, savePath + "/../pdb/" + savePath.GetFileOrFolderName() + "_" + pdb.GetFileOrFolderName());
						File.Delete(pdb);
					}
				}
			}
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);

			int totThreads = threads.Count;
			while (threads.Any())
			{
				EditorUtility.DisplayProgressBar("Waiting for background zipping to finish", (totThreads - threads.Count + 1) + " of " + (totThreads), (totThreads - threads.Count + 1) / totThreads);
				threads.Dequeue().Join();

			}
			EditorUtility.ClearProgressBar();


		}

	}
}
