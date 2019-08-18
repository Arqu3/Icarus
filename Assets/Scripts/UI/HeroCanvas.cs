using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

[AddComponentMenu("")]
public class HeroCanvas : InstantiatableCanvas
{
    public Image original;
    public TMP_Dropdown sortModeDropdown;

    HeroSortMode currentSortmode = HeroSortMode.None;
    List<EventButton> createdButtons = new List<EventButton>();

    public readonly GenericUnityEvent<Hero> OnHeroSelected = new GenericUnityEvent<Hero>();
    public readonly GenericUnityEvent<Hero> OnHeroInspected = new GenericUnityEvent<Hero>();

    const string PlayerPrefsName = "HeroSortMode";

    private void Awake()
    {
        original.gameObject.SetActive(false);

        var list = System.Enum.GetValues(typeof(HeroSortMode));
        var stringList = new List<string>();
        foreach (var e in list) stringList.Add(e.ToString());

        sortModeDropdown.AddOptions(stringList);

        int last = PlayerPrefs.GetInt(PlayerPrefsName, 0);
        currentSortmode = (HeroSortMode)last;
        sortModeDropdown.value = last;

        sortModeDropdown.onValueChanged.AddListener((i) =>
        {
            currentSortmode = (HeroSortMode)i;
            PlayerPrefs.SetInt(PlayerPrefsName, i);
            UpdateList();
        });
    }

    public void UpdateList()
    {
        ClearButtons();

        foreach (var h in HeroCollection.Instance.GetRecruited(currentSortmode))
        {
            var b = HeroUIHelper.CreateHeroEventButton(original, h);
            createdButtons.Add(b);

            var eb = b.gameObject.AddComponent<EventButton>();
            eb.OnClick.AddListener(() => OnHeroSelected.Invoke(h));
            eb.OnRightClick.AddListener(() => OnHeroInspected.Invoke(h));
        }
    }

    public void ClearButtons()
    {
        foreach (var b in createdButtons) Destroy(b.gameObject);
        createdButtons.Clear();
    }
}