using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatProvider : BaseProvider, IStatProvider
{
    public abstract int GetPower();
    public abstract float GetResourceGain();
    public abstract float GetActionCooldown();
    public abstract float GetRange();
    public abstract int GetProjectileCount();
}

public interface IStatProvider
{
    int GetPower();
    float GetResourceGain();
    float GetActionCooldown();
    float GetRange();
    int GetProjectileCount();
}