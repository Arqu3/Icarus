using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class RecruitCanvas : InstantiatableCanvas
{
    public Image baseRecruitImage;
    public Button back;
    public GameObject inspectorPanel;
    public Button recruitButton;
    public TMP_Text description;

    List<EventButton> createdButtons = new List<EventButton>();
    public Hero currentlySelected;

    private void Awake()
    {
        baseRecruitImage.gameObject.SetActive(false);
    }

    public void CreateButton(Hero hero)
    {
        var b = HeroUIHelper.CreateHeroEventButton(baseRecruitImage, hero);
        createdButtons.Add(b);

        b.OnClick.AddListener(() =>
        {
            if (currentlySelected == hero) SetInspectorPanelState(!inspectorPanel.activeSelf);
            else
            {
                SetInspectorPanelState(true);
                currentlySelected = hero;
                description.text = hero.GetDescription();
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
    //public static Button CreateHeroButton(Button original, Hero hero)
    //{
    //    var b = Object.Instantiate(original, original.transform.parent);
    //    b.gameObject.SetActive(true);
    //    b.GetComponentInChildren<TMP_Text>().text = hero.Prefab.name;
    //    return b;
    //}

    public static EventButton CreateHeroEventButton(Image original, Hero hero)
    {
        var b = Object.Instantiate(original, original.transform.parent);
        b.gameObject.SetActive(true);
        b.GetComponentInChildren<TMP_Text>().text = hero.Prefab.name;

        var eb = b.GetComponent<EventButton>();
        if (!eb) eb = b.gameObject.AddComponent<EventButton>();

        return eb;
    }

    public static void SetupInspectEvent(HeroInspectUI ui, GenericUnityEvent<Hero> heroEvent)
    {
        heroEvent.AddListener((hero) =>
        {
            if (ui.CurrentHero == hero) ui.Hide();
            else ui.ShowAndSet(hero);
        });
    }
}