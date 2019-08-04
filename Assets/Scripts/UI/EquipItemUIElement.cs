using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipItemUIElement : MonoBehaviour
{
    [SerializeField]
    TMP_Text header, body, extra;

    public void SetHeader(string text)
    {
        header.text = text;
    }

    public void SetBody(string text)
    {
        body.text = text;
    }

    public void SetExtra(string text)
    {
        extra.text = text;
    }

    public void SetItem(EquipItem item)
    {
        SetHeader(item.ToString());

        var stats = item.GetUsedStats();
        string body = "";

        for (int j = 0; j < stats.Length; ++j)
        {
            body += stats[j].type.ToString() + stats[j].value;

            if (j < stats.Length - 1) body += "\n";
        }

        SetBody(body);
        SetExtra("Randomly generated item");
    }
}
