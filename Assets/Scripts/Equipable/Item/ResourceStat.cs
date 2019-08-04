using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceStat : Stat
{
    public override StatType GetSType()
    {
        return StatType.Resource;
    }
}
