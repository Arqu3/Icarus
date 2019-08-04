using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerStat : Stat
{
    public override StatType GetSType()
    {
        return StatType.Power;
    }
}
