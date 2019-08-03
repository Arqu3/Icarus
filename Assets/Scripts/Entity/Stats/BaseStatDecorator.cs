﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatDecorator : BaseStatProvider
{
    public BaseStatProvider provider;

    public BaseStatDecorator(BaseStatProvider provider)
    {
        this.provider = provider;
    }

    public override float GetActionCooldown()
    {
        return provider.GetActionCooldown();
    }

    public override int GetPower()
    {
        return provider.GetPower();
    }

    public override float GetResourceGain()
    {
        return provider.GetResourceGain();
    }

    public override float GetRange()
    {
        return provider.GetRange();
    }
}
