using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLoot
{
    public MissionLoot(EquipableItem[] loot)
    {
        Get = loot;
    }

    public EquipableItem[] Get { get; }
}
