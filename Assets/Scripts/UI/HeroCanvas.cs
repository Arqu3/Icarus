using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;

[AddComponentMenu("")]
public class HeroCanvas : InstantiatableCanvas
{
    public Button originalButton;

    List<Button> createdButtons = new List<Button>();

    public readonly GenericUnityEvent<Hero> OnHeroSelected = new GenericUnityEvent<Hero>();

    private void Awake()
    {
        originalButton.gameObject.SetActive(false);
    }

    public void UpdateList()
    {
        ClearButtons();

        foreach (var h in HeroCollection.Instance.GetRecruited())
        {
            var b = HeroUIHelper.CreateHeroButton(originalButton, h);
            createdButtons.Add(b);
            b.onClick.AddListener(() => OnHeroSelected.Invoke(h));
        }
    }

    public void ClearButtons()
    {
        foreach (var b in createdButtons) Destroy(b.gameObject);
        createdButtons.Clear();
    }
}