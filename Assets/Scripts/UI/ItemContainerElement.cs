using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Spark.UI;

[RequireComponent(typeof(Image))]
public class ItemContainerElement : MonoBehaviour
{
    Image image;
    public EquipableItem Item { get; private set; }
    EventTrigger trigger;
    RectTransform rTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        rTransform = GetComponent<RectTransform>();

        SetItem(null);

        trigger = GetComponent<EventTrigger>();

        trigger.AddTriggerEvent(EventTriggerType.PointerEnter, () =>
        {
            if (Item != null) ItemTooltipUI.Instance.SetItem(Item, rTransform.position);
        });

        trigger.AddTriggerEvent(EventTriggerType.PointerExit, () =>
        {
            if (Item != null) ItemTooltipUI.Instance.Clear();
        });
    }

    public void SetItem(EquipableItem item)
    {
        Item = item;

        if (!image) image = GetComponent<Image>();

        if (item != null)
        {
            switch (item.rarity)
            {
                case ItemRarity.Common:
                    image.color = Color.white;
                    break;
                case ItemRarity.Rare:
                    image.color = Color.blue;
                    break;
                case ItemRarity.Legendary:
                    image.color = new Color(1f, 0.5f, 0f, 1f);
                    break;
                default:
                    break;
            }
        }
        else image.color = Color.gray;
    }
}