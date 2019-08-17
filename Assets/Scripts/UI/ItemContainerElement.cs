using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Spark.UI;

[RequireComponent(typeof(Image))]
public class ItemContainerElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Image image;
    public EquipableItem Item { get; private set; }
    EventTrigger trigger;
    RectTransform rTransform;
    public readonly GenericUnityEvent<EquipableItem, EquipableItem> OnItemChanged = new GenericUnityEvent<EquipableItem, EquipableItem>();

    private void Awake()
    {
        image = GetComponent<Image>();
        rTransform = GetComponent<RectTransform>();

        //trigger = GetComponent<EventTrigger>();
        //if (!trigger) trigger = gameObject.AddComponent<EventTrigger>();

        //trigger.AddTriggerEvent(EventTriggerType.PointerEnter, () =>
        //{
        //    if (Item != null) ItemTooltipUI.Instance.SetItem(Item, rTransform.position);
        //});

        //trigger.AddTriggerEvent(EventTriggerType.PointerExit, () =>
        //{
        //    if (Item != null) ItemTooltipUI.Instance.Clear();
        //});

        //trigger.AddTriggerEvent(EventTriggerType.PointerClick, () =>
        //{
        //    if (ItemManager.Instance.fromElement == null && Item == null) return;

        //    Debug.Log("event");
        //    if (ItemManager.Instance.fromElement == null) ItemManager.Instance.fromElement = this;
        //    else if (ItemManager.Instance.toElement == null)
        //    {
        //        ItemManager.Instance.toElement = this;
        //        ItemManager.Instance.TransferItem();
        //    }
        //});
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        UpdateColors();
    }

    /// <summary>
    /// Use this to override held item without invoking events
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(EquipableItem item)
    {
        Item = item;
        UpdateColors();
    }

    /// <summary>
    /// Use this to change item and invoke events
    /// </summary>
    /// <param name="item"></param>
    public void GiveItem(EquipableItem item)
    {
        OnItemChanged.Invoke(Item, item);
        SetItem(item);
    }

    void UpdateColors()
    {
        if (!image) image = GetComponent<Image>();

        if (Item != null)
        {
            switch (Item.rarity)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null) ItemTooltipUI.Instance.SetItem(Item, rTransform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item != null) ItemTooltipUI.Instance.Clear();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!ItemManager.Instance.fromElement && Item == null) return;

        if (!ItemManager.Instance.fromElement) ItemManager.Instance.fromElement = this;
        else if (!ItemManager.Instance.toElement)
        {
            ItemManager.Instance.toElement = this;
            ItemManager.Instance.TransferItem();
        }
    }
}