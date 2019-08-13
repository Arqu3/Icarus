using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class RecruitCanvas : InstantiatableCanvas
{
    public Button baseRecruitButton;
    public Button back;
    public GameObject inspectorPanel;
    public Button recruitButton;

    List<Button> createdButtons = new List<Button>();
    public Hero currentlySelected;

    private void Awake()
    {
        baseRecruitButton.gameObject.SetActive(false);
    }

    public void CreateButton(Hero hero)
    {
        var b = HeroUIHelper.CreateHeroButton(baseRecruitButton, hero);
        createdButtons.Add(b);

        b.onClick.AddListener(() =>
        {
            if (currentlySelected == hero) SetInspectorPanelState(!inspectorPanel.activeSelf);
            else
            {
                SetInspectorPanelState(true);
                currentlySelected = hero;
            }
        });
    }

    public void SetInspectorPanelState(bool active)
    {
        inspectorPanel.SetActive(active);
    }

    public void ClearButtons()
    {
        currentlySelected = null;
        foreach (var b in createdButtons) Destroy(b.gameObject);
        createdButtons.Clear();
    }
}

public static class HeroUIHelper
{
    public static Button CreateHeroButton(Button original, Hero hero)
    {
        var b = Object.Instantiate(original, original.transform.parent);
        b.gameObject.SetActive(true);
        b.GetComponentInChildren<TMP_Text>().text = hero.Prefab.name;
        return b;
    }
}