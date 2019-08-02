using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatProvider
{
    public abstract int GetPower();
    public abstract float GetResourceGain();
    public abstract float GetActionCooldown();
    public abstract float GetRange();
}
