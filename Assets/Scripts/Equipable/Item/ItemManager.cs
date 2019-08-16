using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    public ItemContainerElement fromElement;
    public ItemContainerElement toElement;

    public void TransferItem()
    {
        if (fromElement.Item == null)
        {
            Debug.LogError("Item is null!");
            return;
        }

        if (fromElement == toElement)
        {
            fromElement = toElement = null;
            return;
        }

        if (toElement.Item != null)
        {
            var item = toElement.Item;
            toElement.SetItem(fromElement.Item);
            fromElement.SetItem(item);
        }
        else
        {
            toElement.SetItem(fromElement.Item);
            fromElement.SetItem(null);
        }

        fromElement = toElement = null;
        ItemTooltipUI.Instance.Clear();
    }

    int counter = 0;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ++counter;
            StartCoroutine(_DelayedResetCheck());
        }
    }

    IEnumerator _DelayedResetCheck()
    {
        yield return new WaitForEndOfFrame();

        if (counter >= 2)
        {
            if (fromElement && !toElement) fromElement = toElement = null;
            counter = 0;
        }
        else if (!fromElement && !toElement) counter = 0;
    }
}
