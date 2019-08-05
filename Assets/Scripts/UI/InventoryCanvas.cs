using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;
using System.Linq;

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

    }

#if UNITY_EDITOR

    List<EquipItemUIElement> testElements = new List<EquipItemUIElement>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            var hero = FindObjectsOfType<HeroEntity>().Random();
            testElements.ForEach(x => x.gameObject.SetActive(false));
            Test((from s in hero.EquipmentSlots where s.Current != null select s.Current).ToArray());
        }
        if (Input.GetKeyDown(KeyCode.I)) testElements.ForEach(x => x.gameObject.SetActive(false));
    }

    void Test(EquipableItem[] items)
    {
        Vector3 from = new Vector3(-500, 0, 0);
        Vector3 to = new Vector3(500, 0, 0);

        int amount = items.Length;

        for (int i = 0; i < amount; ++i)
        {
            var element = i >= testElements.Count ? CreateElement() : testElements[i];
            element.gameObject.SetActive(true);
            element.SetItem(items[i]);

            element.transform.localPosition = Vector3.Lerp(from, to, (float)i / Mathf.Max(amount - 1, 1));
        }
    }

#endif

    EquipItemUIElement CreateElement()
    {
        var ele = Instantiate(baseElement, baseElement.transform.parent);
        ele.gameObject.SetActive(true);
#if UNITY_EDITOR
        testElements.Add(ele);
#endif
        return ele;
    }
}