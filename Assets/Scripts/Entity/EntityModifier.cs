using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityModifier
{
    BaseEntityHealthProvider healthProvider;
    BaseEntityResourceProvider resourceProvider;
    BaseStatProvider statProvider;

    bool allowHealth, allowResource, allowStat;

    public EntityModifier(BaseEntityHealthProvider healthProvider, BaseEntityResourceProvider resourceProvider, BaseStatProvider statProvider)
    {
        this.statProvider = statProvider;
        this.healthProvider = healthProvider;
        this.resourceProvider = resourceProvider;

        allowHealth = healthProvider != null;
        allowResource = resourceProvider != null;
        allowStat = statProvider != null;
    }

    List<RangedStatDecorator> statDecorators = new List<RangedStatDecorator>();

    public void ApplyStatDecorator()
    {
        statDecorators.Add(new RangedStatDecorator(statDecorators.Last()));
    }

    public void ApplyProjectileDecorator(int extra)
    {
        statDecorators.Add(new AdditiveRangedStatDecorator((statDecorators.Count > 0 ? statDecorators.Last() : statProvider) as BaseRangedStatProvider, extra));
    }

    public void RemoveProjectileDecorator()
    {
        if (statDecorators.Count > 0) statDecorators.RemoveAt(statDecorators.Count() - 1);
    }

    public void RemoveProjectileDecoratorTest()
    {
        if (statDecorators.Count > 1)
        {
            statDecorators[2].provider = statDecorators[0];

            statDecorators.RemoveAt(1);
        }
    }

    public BaseStatProvider GetCurrentStatProvider()
    {
        return statDecorators.Count > 0 ? statDecorators.Last() : statProvider;
    }
}
