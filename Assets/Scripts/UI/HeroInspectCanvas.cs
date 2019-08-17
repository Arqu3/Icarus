using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class HeroInspectCanvas : InstantiatableCanvas
{
    public Button closeButton;
    public TMP_Text descriptionText;
    public ItemContainerElement baseItemslot;
    Hero currentHero;
    List<ItemContainerElement> createdElements = new List<ItemContainerElement>();

    private void Awake()
    {
        baseItemslot.gameObject.SetActive(false);
        for(int i = 0; i < Hero.ITEMSLOTS; ++i)
        {
            var b = Instantiate(baseItemslot, baseItemslot.transform.parent);
            b.gameObject.SetActive(true);
            createdElements.Add(b);
            b.OnItemChanged.AddListener((oldI, newI) =>
            {
                if (currentHero == null) return;
                currentHero.Items.Remove(oldI);
                if (newI != null) currentHero.Items.Add(newI);
            });
        }
    }

    private void OnDisable()
    {
        currentHero = null;
        createdElements.ForEach(x => x.SetItem(null));
    }

    public void ShowHero(Hero hero)
    {
        descriptionText.text = hero.GetDescription();
        createdElements.ForEach(x => x.SetItem(null));
        for (int i = 0; i < hero.Items.Count; ++i) createdElements[i].SetItem(hero.Items[i]);
        currentHero = hero;
    }
}