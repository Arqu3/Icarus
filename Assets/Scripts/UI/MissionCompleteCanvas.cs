using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;
using UnityEngine.SceneManagement;

[AddComponentMenu("")]
public class MissionCompleteCanvas : InstantiatableCanvas
{
    public TMP_Text headerText;
    public GameObject rewardPanel;
    public Button quitButton;
    public ItemContainerElement baseItemContainer;

    private void Awake()
    {
        baseItemContainer.gameObject.SetActive(false);
        quitButton.onClick.AddListener(() => SceneManager.LoadScene("Hub"));
    }

    public void Defeat()
    {
        headerText.text = "Defeat";
        rewardPanel.gameObject.SetActive(false);
    }

    public void Victory(MissionLoot loot)
    {
        headerText.text = "Victory!";

        rewardPanel.gameObject.SetActive(true);
        foreach(var item in loot.Get)
        {
            var ic = Instantiate(baseItemContainer, baseItemContainer.transform.parent);
            ic.gameObject.SetActive(true);
            ic.SetItem(item);
        }
    }
}