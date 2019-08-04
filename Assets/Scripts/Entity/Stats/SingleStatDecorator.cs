using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStatDecorator : BaseStatDecorator
{
    float value;
    int intValue;

    ModMathType mathType;
    StatType statType;

    public SingleStatDecorator(BaseStatProvider provider, StatType statType, ModMathType mathType, float value) : base(provider)
    {
        this.statType = statType;
        this.mathType = mathType;
        this.value = value;
        intValue = (int)value;
    }

    public override float GetActionCooldown()
    {
        if (statType != StatType.ActionCooldown) return base.GetActionCooldown();
        else return mathType == ModMathType.Additive ? base.GetActionCooldown() + value : base.GetActionCooldown() * value;
    }

    public override int GetPower()
    {
        if (statType != StatType.Power) return base.GetPower();
        else return mathType == ModMathType.Additive ? base.GetPower() + intValue : Mathf.CeilToInt(base.GetActionCooldown() * value);
    }

    public override float GetResourceGain()
    {
        if (statType != StatType.Resource) return base.GetResourceGain();
        else return mathType == ModMathType.Additive ? base.GetResourceGain() + value : base.GetResourceGain() * value;
    }
}
