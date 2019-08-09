using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStatDecorator : BaseStatDecorator
{
    float value;
    int intValue;

    public ModMathType MathType { get; private set; }
    StatType statType;

    public SingleStatDecorator(BaseStatProvider provider, StatType statType, ModMathType mathType, float value) : base(provider)
    {
        this.statType = statType;
        MathType = mathType;
        this.value = value;
        intValue = (int)value;
    }

    public override float GetActionCooldown()
    {
        if (statType != StatType.ActionCooldown) return base.GetActionCooldown();
        else return MathType == ModMathType.Additive ? base.GetActionCooldown() + value : base.GetActionCooldown() * value;
    }

    public override int GetPower()
    {
        if (statType != StatType.Power) return base.GetPower();
        else return MathType == ModMathType.Additive ? base.GetPower() + intValue : Mathf.CeilToInt(base.GetActionCooldown() * value);
    }

    public override float GetResourceGain()
    {
        if (statType != StatType.Resource) return base.GetResourceGain();
        else return MathType == ModMathType.Additive ? base.GetResourceGain() + value : base.GetResourceGain() * value;
    }

    public override float GetRange()
    {
        if (statType != StatType.Range) return base.GetRange();
        else return MathType == ModMathType.Additive ? base.GetRange() + value : base.GetRange() * value;
    }
}
