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

    List<Button> createdButtons = new List<Button>();
    HeroRepresentation currentlySelected;

    private void Awake()
    {
        baseRecruitButton.gameObject.SetActive(false);
    }

    public void CreateButton(HeroRepresentation rep)
    {
        var b = Instantiate(baseRecruitButton, baseRecruitButton.transform.parent);
        b.gameObject.SetActive(true);
        b.GetComponentInChildren<TMP_Text>().text = rep.Prefab.name;
        createdButtons.Add(b);

        b.onClick.AddListener(() =>
        {
            if (currentlySelected == rep) SetInspectorPanelState(!inspectorPanel.activeSelf);
            else
            {
                SetInspectorPanelState(true);
                currentlySelected = rep;
            }
        });
    }

    public void SetInspectorPanelState(bool active)
    {
        inspectorPanel.SetActive(active);
    }

    public void ClearButtons()
    {
        foreach (var b in createdButtons) Destroy(b.gameObject);
        createdButtons.Clear();
    }
}