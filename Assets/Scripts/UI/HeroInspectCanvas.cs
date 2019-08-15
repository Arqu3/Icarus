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

    public void ShowHero(Hero hero)
    {
        descriptionText.text = hero.GetDescription();
    }
}