using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Spark.UI
{
	/*[InitializeOnLoad]
	class CanvasSaver
	{


		static CanvasSaver()
		{
			UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += EditorSceneManager_sceneSaving;
		}

		private static void EditorSceneManager_sceneSaving( UnityEngine.SceneManagement.Scene scene, string path )
		{
			if ( !scene.IsValid()  || scene.path == null)
				return;
			if ( scene.path.StartsWith( "Assets/Scenes/UI/" ) ) // in a UI editing scene
			{
				foreach ( var c in GameObject.FindObjectsOfType<InstantiatableCanvas>() )
				{
					System.IO.Directory.CreateDirectory( string.Format( InstantiatableCanvas.FullResourcePath, "" ) );
					PrefabUtility.CreatePrefab( string.Format( InstantiatableCanvas.RelativePath, c.gameObject.name + ".prefab" ), c.gameObject );
				}
			}
		}
	}*/
}

