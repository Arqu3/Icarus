using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class EmbarkCanvas : InstantiatableCanvas
{
    public TMP_Text rosterText;
    public Button back, start, baseRosterButton;

    List<Button> selectedHeroes = new List<Button>();
    const int MAXIMUM_HEROES = 4;

    string rosterFormat;

    private void Awake()
    {
        baseRosterButton.gameObject.SetActive(false);
        rosterFormat = rosterText.text;
        UpdateRosterText();
        start.interactable = false;
    }

    public bool AddToRoster(Hero hero)
    {
        if (selectedHeroes.Count >= MAXIMUM_HEROES) return false;

        hero.state = HeroState.Selected;

        ChangeRoster();
        return true;
    }

    public void RemoveFromRoster(Hero hero)
    {
        hero.state = HeroState.Recruited;
        ChangeRoster();
    }

    public void ChangeRoster()
    {
        ClearRoster();

        foreach (var h in HeroCollection.Instance.GetSelected())
        {
            var b = HeroUIHelper.CreateHeroButton(baseRosterButton, h);
            b.onClick.AddListener(() => RemoveFromRoster(h));
            selectedHeroes.Add(b);
        }
        start.interactable = selectedHeroes.Count > 0;
        UpdateRosterText();
    }

    void UpdateRosterText()
    {
        rosterText.text = string.Format(rosterFormat, selectedHeroes.Count, MAXIMUM_HEROES);
    }

    public void ClearRoster()
    {
        foreach (var b in selectedHeroes) Destroy(b.gameObject);
        selectedHeroes.Clear();
    }
}