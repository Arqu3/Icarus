using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor.SceneManagement;
using System.Reflection;
using System.Linq;
public class CanvasCreator : EditorWindow {

	private string canvasName, canvasInstanceName, uiInstanceName;
	private int stage = -1;

	[MenuItem("Assets/Create/Canvas")]
	private static void CreateCanvas()
	{
		var win = EditorWindow.GetWindow(typeof(CanvasCreator), false, null, true);
		win.Show(true);
	}

	private void Update()
	{
		try
		{
			switch ( stage )
			{
				case 0:
					EditorUtility.DisplayProgressBar( "Creating canvas " + canvasInstanceName, "Generating scripts", 0f );
					Directory.CreateDirectory( Application.dataPath + "/Scripts/UI" );
					File.WriteAllText( Application.dataPath + "/Scripts/UI/" + canvasInstanceName + ".cs", Regex.Replace( canvasCode, "(%Canvas%)", canvasInstanceName ) );
					File.WriteAllText( Application.dataPath + "/Scripts/UI/" + uiInstanceName + ".cs", Regex.Replace( Regex.Replace( uiCode, "(%Canvas%)", canvasInstanceName ), "(%UI%)", uiInstanceName ));
					AssetDatabase.Refresh();
					EditorUtility.DisplayProgressBar("Creating canvas " + canvasInstanceName, "Compiling", 0.5f );
					stage = 1;
					return;
				case 1:
					if ( !EditorApplication.isCompiling )
						stage = 2;
					break;
				case 2:
					EditorUtility.DisplayProgressBar("Creating canvas " + canvasInstanceName, "Creating scene", 1f );
					Directory.CreateDirectory( Application.dataPath + "/Resources/UI/Instantiatable" );
					AssetDatabase.Refresh();

					var t = Assembly.Load("Assembly-CSharp").GetType(canvasInstanceName);
					var o = new GameObject(canvasInstanceName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), t);
					o.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
					o.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
					o.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
					o.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
					o.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
					//PrefabUtility.CreatePrefab(Application.dataPath + "/Resources/UI/Instantiatable/" + o.name + ".prefab", o);
					var prefab = PrefabUtility.SaveAsPrefabAsset(o, Application.dataPath + "/Resources/UI/Instantiatable/" + o.name + ".prefab");

					DestroyImmediate(o);

					EditorUtility.ClearProgressBar();
					stage = 3;
					AssetDatabase.Refresh();
					
					AssetDatabase.OpenAsset(prefab);
					ReloadSceneMenu();
					Close();

					break;
			}
		}
		catch ( System.Exception e )
		{
			Debug.LogException(e);
			EditorUtility.ClearProgressBar();
		}
	}

	void OnGUI()
	{
		canvasName = EditorGUILayout.TextField( "Prefab Name", canvasName );
		if ( GUILayout.Button( "Create" ) && stage == -1 && canvasName != null && !string.IsNullOrEmpty( canvasName = Regex.Replace(Regex.Replace( canvasName, @"\s", "" ), "Canvas$", "")))
		{
			stage = 0;
			canvasName = char.ToUpper(canvasName[0]) + canvasName.Substring(1);
			canvasInstanceName = canvasName + "Canvas";
			uiInstanceName = canvasName + "UI";
			
		}

	}

	private const string canvasCode = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;

[AddComponentMenu("""")]
public class %Canvas% : InstantiatableCanvas {
	// Add public fields here for buttons and stuff
}";
	private const string uiCode = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;

public class %UI% : InstantiatableUI<%UI%, %Canvas%>
{

	public %UI%( UnityAction<%UI%> configure = null ) : base( configure )
	{
		
	}
}";
	static readonly string Path = "Plugins/Spark/Scripts/Editor/";
	static readonly string ClassName = "UIToMenuItems.cs";
	static readonly string AltPath = "Editor/";
	
	private static void ReloadSceneMenu()
	{
		if (Directory.Exists(Application.dataPath + "/" + Path)) GenerateMenuItemFile(Path + ClassName);
		else if (Directory.Exists(Application.dataPath + "/" + AltPath)) GenerateMenuItemFile(AltPath + ClassName);
		else Debug.LogWarning("Could not generate scene menu, neither path '" + Path + "' or '" + AltPath + "' were found");
	}

	static void GetScenesRecursively(ref List<string> result, string path)
	{
		foreach (var item in Directory.GetDirectories(path).Select(x => x.Substring(x.LastIndexOf("/") + 1)))
		{
			GetScenesRecursively(ref result, path + item + "/");
		}
		result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".prefab")).Select(x => x.Substring(x.LastIndexOf("UI") + 3)).Select(x => x.Substring(0, x.LastIndexOf("."))).ToArray());
	}

	static void GenerateMenuItemFile(string pathMinusADP)
	{
		List<string> availableScenes = new List<string>();
		GetScenesRecursively(ref availableScenes, Application.dataPath + "/Resources/UI/");
		if (File.Exists(Application.dataPath + "/" + pathMinusADP))
		{
			File.Delete(Application.dataPath + "/" + pathMinusADP);
		}

		List<string> lines = new List<string>();

		lines.Add("//this file is autogenerated on the date " + System.DateTime.Now);
		lines.Add("using UnityEngine;");
		lines.Add("using UnityEditor;");
		lines.Add("");
		lines.Add("public static class UIToMenuItems");
		lines.Add("{");

		foreach (var item in availableScenes)
		{
			lines.Add("\t[MenuItem(\"UI/" + item + "\")]");
			lines.Add("\tprivate static void UI_" + item.ToUpper()
				.Replace(' ', '_')
				.Replace('/', '_')
				.Replace("(", "")
				.Replace(")", "")
				.Replace('-', '_')
				.Replace(",", "")
				.Replace(".", "")
				.Replace('@', '_')
				.Replace('!', '_')
				.Replace('?', '_')
				.Replace("'", "")
				.Replace('"', '_')
				.Replace("<", "")
				.Replace(">", "")
				.Replace("|", "")
				.Replace("*", "")
				+ " ()");
			lines.Add("\t{");
			//lines.Add("\t\tUnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();");
			//lines.Add("\t\tUnityEditor.SceneManagement.EditorSceneManager.OpenScene (\"Assets/Scenes/" + item + ".unity\");");
			lines.Add("\t\tUnityEditor.AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(\"Assets/Resources/UI/" + item + ".prefab\", typeof(Object)));");
			lines.Add("\t}");
			lines.Add("");
		}

		lines.Add("}");


		File.WriteAllLines(Application.dataPath + "/" + pathMinusADP, lines.ToArray());
		AssetDatabase.ImportAsset("Assets/" + pathMinusADP);
	}
}
