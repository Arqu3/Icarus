using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

    public void SetItem(EquipableItem item)
    {
        SetHeader(item.ToString() + "\n" + item.rarity);

        var stats = item.GetUsedStats().ToList();
        string body = "";

        var sameMath = stats.GroupBy(x => x.mathType).Where(y => y.Count() > 1).Select(y => y.ToArray()).ToArray();
        List<StatStruct[]> pairs = new List<StatStruct[]>();
        foreach (var stat in sameMath)
        {
            var pair = stat.GroupBy(x => x.type).Where(y => y.Count() > 1).Select(y => y.ToArray()).ToArray();
            foreach(var p in pair) pairs.Add(p);
        }

        foreach (var p in pairs)
        {
            foreach(var s in p) stats.Remove(s);

            bool add = p.First().mathType == ModMathType.Additive;

            float sum = add ? p.Select(x => x.value).Sum() : p.Select(x => x.value).Aggregate(1f, (y, z) => y * z);
            StatType type = p.First().type;

            string toAdd = "";

            if (add) toAdd += Additive(sum);
            else toAdd += Multiplicative(sum);

            switch (p.First().type)
            {
                case StatType.Health:
                    toAdd += " Health";
                    break;
                case StatType.ActionCooldown:
                    toAdd += " Action Cooldown";
                    break;
                case StatType.Resource:
                    toAdd += " Resource Gain";
                    break;
                case StatType.Power:
                    toAdd += " Power";
                    break;
                default:
                    break;
            }

            body += toAdd;
            body += "\n";
        }

        for (int i = 0; i < stats.Count; ++i)
        {
            var stat = stats[i];
            bool add = stat.mathType == ModMathType.Additive;
            string toAdd = "";

            if (add) toAdd += Additive(stat.value);
            else toAdd += Multiplicative(stat.value);

            switch (stat.type)
            {
                case StatType.Health:
                    toAdd += " Health";
                    break;
                case StatType.ActionCooldown:
                    toAdd += " Action Cooldown";
                    break;
                case StatType.Resource:
                    toAdd += " Resource Gain";
                    break;
                case StatType.Power:
                    toAdd += " Power";
                    break;
                default:
                    break;
            }

            body += toAdd;
            if (i < stats.Count - 1) body += "\n";
        }

        SetBody(body);
        SetExtra("Randomly generated item");
    }

    #region Help functions

    string Multiplicative(float value)
    {
        float formattedValue = Mathf.Abs((value - 1f) * 100f);
        return formattedValue + "%" + (value > 1f ? " Increased" : " Reduced");
    }

    string Additive(float value)
    {
        return (value > 0 ? "+" : "") + value;
    }

    #endregion
}
