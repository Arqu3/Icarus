using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityResourceProvider : BaseProvider, IEntityResourceProvider
{
    public abstract float GetCurrent();
    public abstract float GetMax();
    public abstract float GetPercentage();

    public abstract void Give(float amount);
    public abstract void GivePercentage(float percentage);

    public abstract bool Spend(float amount);
    public abstract bool SpendPercentage(float percentage);

    public abstract void Update();
}
