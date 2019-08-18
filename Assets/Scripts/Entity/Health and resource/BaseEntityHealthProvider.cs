using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityHealthProvider : BaseProvider, IEntityHealthProvider
{
    public abstract int GetCurrent();
    public abstract int GetMax();
    public abstract float GetPercentage();

    public abstract void Give(int amount);
    public abstract void GivePercentage(float percentage);

    public abstract DamageResult Remove(int amount, DamageType type);
    public abstract DamageResult RemovePercentage(float percentage, DamageType type);

    public abstract int GetRegenAmount();
    public abstract float GetRegenInterval();
}
