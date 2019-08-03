using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAddDecorator : HealthDecorator
{
    int extra;

    public HealthAddDecorator(BaseEntityHealthProvider provider, int extra) : base(provider)
    {
        this.extra = extra;
    }

    public override int GetMax()
    {
        return base.GetMax() + extra;
    }
}
