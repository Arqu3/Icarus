using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMultiDecorator : HealthDecorator
{
    float multi;

    public HealthMultiDecorator(BaseEntityHealthProvider provider, float multi) : base(provider)
    {
        this.multi = multi;
    }

    public override int GetMax()
    {
        return Mathf.CeilToInt(base.GetMax() * multi);
    }
}
