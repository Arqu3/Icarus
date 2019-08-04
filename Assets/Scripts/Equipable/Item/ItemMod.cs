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

    public Mod health;
    public Mod power;
    public Mod resourceGain;
    public Mod actionCooldown;

    public Mod[] GetAllMods => new[] { health, power, resourceGain, actionCooldown };
}
