using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CooldownStat : Stat
{
    public override StatType GetSType()
    {
        return StatType.ActionCooldown;
    }
}
