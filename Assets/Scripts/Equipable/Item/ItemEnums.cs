﻿public enum ModType
{
    Prefix = 0,
    Suffix = 1
}

public enum StatType
{
    Health = 0,
    ActionCooldown = 1,
    Resource = 2,
    Power = 3
}

public enum ModMathType
{
    Additive = 0,
    Multiplicative = 1
}

public enum ValueType
{
    Int = 0,
    Float = 1
}

public enum InventoryInteraction
{
    Invalid = -1,
    Give = 0,
    Take = 1,
    Swap = 2,
    Delete = 4
}

public enum Rarity
{
    Common = 0,
    Rare = 1,
    Legendary = 2
}