using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public readonly UnityEvent OnClick = new UnityEvent();
    public readonly UnityEvent OnRightClick = new UnityEvent();

    Color baseColor;
    Color hoverColor;

    void Awake()
    {
        baseColor = GetComponent<Image>().color;
        hoverColor = baseColor * 0.8f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) OnClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right) OnRightClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().CrossFadeColor(hoverColor, 0.1f, true, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().CrossFadeColor(baseColor, 0.1f, true, false);
    }
}
