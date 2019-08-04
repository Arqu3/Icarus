using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemMod
{
#if UNITY_EDITOR

    public bool debugShowUnused;

#endif

    public string name;
    public ModType modType;
    public int tier;

    public HealthStat health;
    public PowerStat power;
    public ResourceStat resourceGain;
    public CooldownStat actionCooldown;

    public Stat[] GetAllStats() => new Stat[] { health, power, resourceGain, actionCooldown };

    public ConvertedStat[] GetUsedStats()
    {
        List<ConvertedStat> stats = new List<ConvertedStat>();

        foreach (var stat in GetAllStats())
        {
            if (!stat.IsUsed()) continue;

            stats.Add(new ConvertedStat { mathType = stat.mathType, type = stat.GetSType(), value = stat.GetValue });
        }

        return stats.ToArray();
    }
}

public struct ConvertedStat
{
    public StatType type;
    public ModMathType mathType;
    public float value;
}