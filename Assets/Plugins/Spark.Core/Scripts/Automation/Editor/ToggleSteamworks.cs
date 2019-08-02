using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToggleSteamworks : EditorWindow
{
    const string symbol = ";DISABLESTEAMWORKS";

    [MenuItem ("Tools/Steam toggler")]
    public static void Init ()
    {
        ToggleSteamworks window = (ToggleSteamworks)GetWindow (typeof (ToggleSteamworks));
        window.Show ();
    }

    private void OnGUI ()
    {
        if ( GUILayout.Button ("Enable steamworks") ) Toggle (false);

        if ( GUILayout.Button ("Disable steamworks") ) Toggle (true);
    }

    public static void Toggle(bool disablesteamworks)
    {
        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.Standalone);

        if (disablesteamworks)
        {
            if ( !symbols.Contains (symbol) )
            {
                string s = symbols + symbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Standalone, s);
                Debug.Log ("Steamworks enabled");
            }
            else Debug.LogWarning ("Steamworks is already enabled");
        }
        else
        {
            if ( symbols.Contains (symbol) )
            {
                string s = symbols.Replace (symbol, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Standalone, s);
                Debug.Log ("Steamworks disabled");
            }
            else Debug.LogWarning ("Steamworks is not enabled");
        }
    }

	public static bool IsSteamworksDisabled()
	{
		var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
		return symbols.Contains(symbol);
	}
}
