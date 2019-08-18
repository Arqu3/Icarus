using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubMenuInitializer : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var inspectUI = new HeroInspectUI();

        var heroUI = new HeroUI(inspectUI);
        heroUI.Show();
        var rui = new RecruitUI(heroUI);

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

        var eUI = new EmbarkUI(heroUI, inspectUI);
        eUI.OnBack.AddListener(() =>
        {
            eUI.Hide();
            hui.Show();
        });

        hui.OnEmbark.AddListener(() =>
        {
            hui.Hide();
            eUI.Show();
        });
    }
}
