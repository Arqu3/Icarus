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

    List<IStatDecorator> statDecorators = new List<IStatDecorator>();

    //public void ApplyStatDecorator()
    //{
    //    statDecorators.Add(new RangedStatDecorator(statDecorators.Last() as BaseRangedStatProvider));
    //}

    public void ApplyProjectileDecorator(int extra)
    {
        statDecorators.Add(new AdditiveRangedStatDecorator(statDecorators.Count > 0 ? statDecorators.Last() as BaseRangedStatProvider : statProvider as BaseRangedStatProvider, extra));
    }

    public void RemoveProjectileDecorator()
    {
        if (statDecorators.Count > 0) RemoveDecoratorAtIndex(statDecorators.Count - 1);
    }

    void RemoveDecoratorAtIndex(int index)
    {
        if (index + 1 < statDecorators.Count && index - 1 >= 0) statDecorators[index + 1].provider = statDecorators[index - 1] as BaseStatProvider;
        else if (index == 0 && statDecorators.Count > 1) statDecorators[index + 1].provider = statProvider as BaseStatProvider;

        statDecorators.RemoveAt(index);
    }

    public BaseStatProvider GetCurrentStatProvider()
    {
        return statDecorators.Count > 0 ? statDecorators.Last() as BaseStatProvider : statProvider;
    }
}
