using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;

[AddComponentMenu("")]
public class InventoryCanvas : InstantiatableCanvas
{
    public EquipItemUIElement baseElement;

    public void Awake()
    {
        baseElement.gameObject.SetActive(false);
    }

    private void Start()
    {
        Vector3 from = new Vector3(-500, 0, 0);
        Vector3 to = new Vector3(500, 0, 0);

        int amount = 3;

        for(int i = 0; i < amount; ++i)
        {
            var element = CreateElement();
            var item = ItemCreator.CreateRandomItem();
            element.SetItem(item);

            element.transform.localPosition = Vector3.Lerp(from, to, (float)i / (amount  - 1));
        }
    }

    EquipItemUIElement CreateElement()
    {
        var ele = Instantiate(baseElement, baseElement.transform.parent);
        ele.gameObject.SetActive(true);
        return ele;
    }
}