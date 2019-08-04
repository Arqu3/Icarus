using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthStat : Stat
{
    public override StatType GetSType()
    {
        return StatType.Health;
    }
}
