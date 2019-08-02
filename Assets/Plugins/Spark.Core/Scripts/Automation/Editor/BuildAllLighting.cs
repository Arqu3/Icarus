using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Spark.IO;
namespace Spark.Automation
{
	public class BuildAllLighting : MonoBehaviour
	{
		[MenuItem("Tools/Lighting/Build All Lighting")]
		public static void BuildLightingAll()
		{
			SceneToolUtil.DoThingForEachSceneInBuildSettings(BuildLighting);

		}

		[MenuItem("Tools/Lighting/Build Lighting This Scene")]
		public static void BuildLightingForCurrent()
		{
			BuildLighting(SceneManager.GetActiveScene());
		}


		private static void BuildLighting(Scene scene)
		{
			//var settings = lightSettings.First(s => System.Text.RegularExpressions.Regex.IsMatch(scene.name, s.match)).settings;
			Lightmapping.Clear();
			Lightmapping.ClearDiskCache();
			Lightmapping.realtimeGI = false;
			Lightmapping.bakedGI = false;
			//EditorUtility.DisplayProgressBar("Processing", scene.name, 0f);
			if (!Lightmapping.Bake()) return;

			EditorSceneManager.MarkAllScenesDirty();
			EditorSceneManager.SaveOpenScenes();
			AssetDatabase.Refresh();

			var objs = (from o in scene.GetRootGameObjects()
						from p in o.GetComponentsInChildren<ReflectionProbe>()
						select p.gameObject).ToArray();//scene.GetRootGameObjects().Select(s => s.GetComponentsInChildren<ReflectionProbe>()).Select( r=> r.;
			for (int j = 0; j < objs.Length; j++)
			{
				if (objs[j].name.Equals("GenericProbe")) continue;
				var probe = objs[j].GetComponent<ReflectionProbe>();
				if (probe)
				{
					DestroyImmediate(probe);
					var p = objs[j].AddComponent<ReflectionProbe>();
					p.mode = UnityEngine.Rendering.ReflectionProbeMode.Custom;
					Lightmapping.BakeReflectionProbe(p, scene.path.RemoveFullExtension() + "/ReflectionProbe-" + j + ".exr");
					AssetDatabase.Refresh();
					p.customBakedTexture = AssetDatabase.LoadAssetAtPath<Texture>(scene.path.RemoveFullExtension() + "/ReflectionProbe-" + j + ".exr");
				}
			}
			var reflection = GameObject.Find("GenericProbe");
			if (!reflection)
			{
				reflection = new GameObject("GenericProbe", typeof(ReflectionProbe));
			}
			{
				reflection = GameObject.Find("GenericProbe");
				DestroyImmediate(reflection.GetComponent<ReflectionProbe>());
				var p = reflection.AddComponent<ReflectionProbe>();
				p.cullingMask = 0;
				p.size = new Vector3(1000, 1000, 1000);
				p.intensity = 1f;
				p.importance = 0;
				p.mode = UnityEngine.Rendering.ReflectionProbeMode.Custom;
				Lightmapping.BakeReflectionProbe(p, scene.path.RemoveFullExtension() + "/ReflectionProbe-global.exr");
				AssetDatabase.Refresh();
				p.customBakedTexture = AssetDatabase.LoadAssetAtPath<Texture>(scene.path.RemoveFullExtension() + "/ReflectionProbe-global.exr");

			}

			EditorSceneManager.MarkAllScenesDirty();
			EditorSceneManager.SaveOpenScenes();
			AssetDatabase.Refresh();
		}
	}
}