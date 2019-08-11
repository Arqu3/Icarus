using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class ItemTooltipCanvas : InstantiatableCanvas
{
    public EquipItemUIElement element;

    private void Start()
    {
        element.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void SetItem(EquipableItem item, Vector2 screenpos)
    {
        element.gameObject.SetActive(true);
        element.SetItem(item);
        element.rectTransform.position = screenpos;
    }

    public void Clear()
    {
        element.gameObject.SetActive(false);
    }
}