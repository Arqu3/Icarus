using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInitializer : MonoBehaviour
{
    private void Awake()
    {
        var main = new MainMenuUI();
        main.Show();

        main.onExit.AddListener(() => Application.Quit());

        main.onStartGame.AddListener(() => SceneManager.LoadScene("Hub"));

        Time.timeScale = 1f;
    }
}
