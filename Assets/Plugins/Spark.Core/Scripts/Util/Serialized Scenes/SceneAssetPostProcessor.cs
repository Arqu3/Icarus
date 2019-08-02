#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

public class SceneAssetPostProcessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var changedScenes = (from p in movedAssets where p.EndsWith(".unity") select p).ToArray();
        if (changedScenes.Length == 0) return;

        var oldChangedScenes = (from p in movedFromAssetPaths where p.EndsWith(".unity") select p).ToArray();

        SceneNameData[] data = new SceneNameData[changedScenes.Length];
        for (int i = 0; i < changedScenes.Length; ++i)
        {
            data[i] = new SceneNameData
            {
                oldName = Regex.Match(oldChangedScenes[i], "[^\\/]+$").Value.Replace(".unity", ""),
                newName = Regex.Match(changedScenes[i], "[^\\/]+$").Value.Replace(".unity", "")
            };
        }
        SerializedSceneContainer.Instance?.EDITORONLY_UpdateInternalNames(data);
    }
}

public struct SceneNameData
{
    public string oldName;
    public string newName;
}

#endif
