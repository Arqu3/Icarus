using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRangeConditionDecorator : BaseStatDecorator
{
    System.Func<bool> condition;
    float extra;

    public StatRangeConditionDecorator(BaseStatProvider provider, float extra, System.Func<bool> condition) : base(provider)
    {
        this.extra = extra;
        this.condition = condition;
    }

    public override float GetRange()
    {
        return condition() ? base.GetRange() + extra : base.GetRange();
    }
}
