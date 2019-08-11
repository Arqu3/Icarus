using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

struct StatText
{
    public StatType type;
    public string text;
}

public class EquipItemUIElement : MonoBehaviour
{
    [SerializeField]
    Color commonColor = Color.white;
    [SerializeField]
    Color rareColor = Color.blue;
    [SerializeField]
    Color legendaryColor = new Color(1f, 0.5f, 0f);

    [SerializeField]
    TMP_Text header, body, extra;

    public RectTransform rectTransform { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

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

    public void SetItem(EquipableItem item)
    {
        ItemStringBuilder.Build(item, out string header, out string body, out string extra);

        switch (item.rarity)
        {
            case ItemRarity.Common:
                this.header.color = commonColor;
                break;
            case ItemRarity.Rare:
                this.header.color = rareColor;
                break;
            case ItemRarity.Legendary:
                this.header.color = legendaryColor;
                break;
            default:
                break;
        }

        SetHeader(header);
        SetBody(body);
        SetExtra(extra);
    }
}

public static class ItemStringBuilder
{
    public static void Build(EquipableItem item, out string header, out string body, out string extra)
    {
        header = "Item\n" + item.rarity;
        extra = "Generated Item";

        var stats = item.GetUsedStats().ToList();
        body = "";
        List<StatText> statTexts = new List<StatText>();

        var sameMath = stats.GroupBy(x => x.mathType).Where(y => y.Count() > 1).Select(y => y.ToArray()).ToArray();
        List<StatStruct[]> pairs = new List<StatStruct[]>();
        foreach (var stat in sameMath)
        {
            var pair = stat.GroupBy(x => x.type).Where(y => y.Count() > 1).Select(y => y.ToArray()).ToArray();
            foreach (var p in pair) pairs.Add(p);
        }

        foreach (var p in pairs)
        {
            foreach (var s in p) stats.Remove(s);

            bool add = p.First().mathType == ModMathType.Additive;

            float sum = add ? p.Select(x => x.value).Sum() : p.Select(x => x.value).Aggregate(1f, (y, z) => y * z);
            StatType type = p.First().type;

            string toAdd = "";

            if (add) toAdd += Additive(sum);
            else toAdd += Multiplicative(sum);

            toAdd += GetStatSuffixText(p.First().type);

            statTexts.Add(new StatText { text = toAdd, type = p.First().type });
        }

        for (int i = 0; i < stats.Count; ++i)
        {
            var stat = stats[i];
            bool add = stat.mathType == ModMathType.Additive;
            string toAdd = "";

            if (add) toAdd += Additive(stat.value);
            else toAdd += Multiplicative(stat.value);

            toAdd += GetStatSuffixText(stat.type);

            statTexts.Add(new StatText { text = toAdd, type = stat.type });
        }

        statTexts = statTexts.OrderBy(x => x.type).ToList();
        for (int i = 0; i < statTexts.Count; ++i)
        {
            body += statTexts[i].text;
            if (i != statTexts.Count - 1) body += "\n";
        }
    }

    #region Help functions

    static string GetStatSuffixText(StatType type)
    {
        string suffix = "";

        switch (type)
        {
            case StatType.Health:
                suffix = " Health";
                break;
            case StatType.ActionCooldown:
                suffix = " Action Cooldown";
                break;
            case StatType.Resource:
                suffix = " Resource Gain";
                break;
            case StatType.Power:
                suffix = " Power";
                break;
            case StatType.Range:
                Debug.LogWarning("NO RANGE");
                break;
            default:
                break;
        }

        return suffix;
    }

    static string Multiplicative(float value)
    {
        float formattedValue = Mathf.Abs((value - 1f) * 100f);
        return formattedValue.ToString("0.##") + "%" + (value > 1f ? " Increased" : " Reduced");
    }

    static string Additive(float value)
    {
        return (value > 0 ? "+" : "") + value.ToString("0.##");
    }

    #endregion
}