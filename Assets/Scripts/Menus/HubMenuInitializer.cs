using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubMenuInitializer : MonoBehaviour
{
    void Awake()
    {
        var ui = new HubUI();
        ui.Show();

        ui.OnExitToMenu.AddListener(() => SceneManager.LoadScene(0));
    }
}
