public enum ModType
{
    Prefix = 0,
    Suffix = 1
}

public enum StatType
{
    Health = 0,
    Power = 1,
    ActionCooldown = 2,
    Resource = 3,
    Range = 4
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

public enum ItemRarity
{
    Common = 0,
    Rare = 1,
    Legendary = 2
}