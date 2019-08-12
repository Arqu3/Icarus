using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubMenuInitializer : MonoBehaviour
{
    void Awake()
    {
        var rui = new RecruitUI();

        var hui = new HubUI();
        hui.Show();

        rui.OnBack.AddListener(() =>
        {
            rui.Hide();
            hui.Show();
        });

        hui.OnApplications.AddListener(() =>
        {
            hui.Hide();
            rui.Show();
        });

        hui.OnExitToMenu.AddListener(() => SceneManager.LoadScene(0));
    }
}
