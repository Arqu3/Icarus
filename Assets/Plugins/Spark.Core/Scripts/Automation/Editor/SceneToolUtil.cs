using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
namespace Spark.Automation
{
	public static class SceneToolUtil
	{

		public static void DoThingForEachSceneInBuildSettings(System.Action<Scene> thingToDo, bool save = true)
		{
			DoThingForEachScene(EditorBuildSettings.scenes, thingToDo, save);
		}

		public static void DoThingForEachScene(EditorBuildSettingsScene[] scenes, System.Action<Scene> thingToDo, bool save = true)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				return;
			}
			try
			{
				AssetDatabase.StartAssetEditing();
				for (int i = 0; i < scenes.Length && !EditorUtility.DisplayCancelableProgressBar("Processing", "scene " + i, i / (float)scenes.Length); i++)
				{
					var s = EditorSceneManager.OpenScene(scenes[i].path, OpenSceneMode.Single);
					thingToDo(s);
					if (save)
						EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
				}
			}
			catch (System.Exception e)
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.StopAssetEditing();
				AssetDatabase.Refresh();
				throw e;
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.StopAssetEditing();
			AssetDatabase.Refresh();
		}

		public static void DoThingForEachScene(Scene[] scenes, System.Action<Scene> thingToDo, bool save = true)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				return;
			}
			try
			{
				AssetDatabase.StartAssetEditing();
				for (int i = 0; i < scenes.Length && !EditorUtility.DisplayCancelableProgressBar("Processing", "scene " + i, i / (float)scenes.Length); i++)
				{
					var s = EditorSceneManager.OpenScene(scenes[i].path, OpenSceneMode.Single);
					thingToDo(s);
					if (save)
						EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
				}
			}
			catch (System.Exception e)
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.StopAssetEditing();
				AssetDatabase.Refresh();
				throw e;
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.StopAssetEditing();
			AssetDatabase.Refresh();
		}
	}
}

