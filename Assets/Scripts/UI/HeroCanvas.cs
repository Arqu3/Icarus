using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("")]
public class HeroCanvas : InstantiatableCanvas
{
    public Image original;

    List<EventButton> createdButtons = new List<EventButton>();

    public readonly GenericUnityEvent<Hero> OnHeroSelected = new GenericUnityEvent<Hero>();
    public readonly GenericUnityEvent<Hero> OnHeroInspected = new GenericUnityEvent<Hero>();

    private void Awake()
    {
        original.gameObject.SetActive(false);
    }

    public void UpdateList()
    {
        ClearButtons();

        foreach (var h in HeroCollection.Instance.GetRecruited())
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