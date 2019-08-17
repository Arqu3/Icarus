using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSingleton : MonoSingleton<MissionSingleton>
{
    MissionLoot loot;

    public void StartNew(IMission mission)
    {
        Current = mission;
    }

    public IMission Current { get; private set; }

    public void GiveLoot(MissionLoot loot)
    {
        this.loot = loot;
    }

    public bool HasLoot() => loot != null;

    public EquipableItem[] GetLoot()
    {
        var items = loot.Get;
        loot = null;
        return items;
    }
}
