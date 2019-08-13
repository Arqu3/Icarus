using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;
using UnityEngine.SceneManagement;

[AddComponentMenu("")]
public class EmbarkCanvas : InstantiatableCanvas
{
    public TMP_Text rosterText;
    public TMP_Text difficultyText;
    public Button back, start, baseRosterButton;
    public Button[] difficultyButtons;

    List<Button> selectedHeroes = new List<Button>();
    const int MAXIMUM_HEROES = 4;

    string rosterFormat;
    string difficultyFormat;

    int difficulty = 0;

    private void Awake()
    {
        baseRosterButton.gameObject.SetActive(false);
        rosterFormat = rosterText.text;
        difficultyFormat = difficultyText.text;
        UpdateRosterText();
        start.interactable = false;

        for(int i = 0; i < difficultyButtons.Length; ++i)
        {
            //So if you don't asign the index to a variable wierd shit happens?
            int index = i;
            difficultyButtons[index].onClick.AddListener(() => SelectDifficulty(index));
        }

        start.onClick.AddListener(() =>
        {
            //Move scene loading to mission singleton?
            MissionSingleton.Instance.StartNew(new Mission(difficulty));
            SceneManager.LoadScene("Arena");
        });
    }

    private void OnEnable()
    {
        SelectDifficulty(0);
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

    void SelectDifficulty(int index)
    {
        difficulty = index;
        difficultyText.text = string.Format(difficultyFormat, difficultyButtons[index].GetComponentInChildren<TMP_Text>().text);
    }
}