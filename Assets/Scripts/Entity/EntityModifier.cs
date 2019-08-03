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

    List<IStatDecorator> statDecorators = new List<IStatDecorator>();
    List<IResourceDecorator> resourceDecorators = new List<IResourceDecorator>();
    List<IHealthDecorator> healthDecorators = new List<IHealthDecorator>();

    public EntityModifier(BaseEntityHealthProvider healthProvider, BaseEntityResourceProvider resourceProvider, BaseStatProvider statProvider)
    {
        this.statProvider = statProvider;
        this.healthProvider = healthProvider;
        this.resourceProvider = resourceProvider;

        allowHealth = healthProvider != null;
        allowResource = resourceProvider != null;
        allowStat = statProvider != null;
    }

    //public void ApplyStatDecorator()
    //{
    //    statDecorators.Add(new RangedStatDecorator(statDecorators.Last() as BaseRangedStatProvider));
    //}

    public void ApplyProjectileDecorator(int extra)
    {
        var provider = statDecorators.Count > 0 ? statDecorators.Last() as BaseStatProvider : statProvider;
        statDecorators.Add(new ExtraProjectilesDecorator(provider as BaseRangedStatProvider, extra));
    }

    public void RemoveProjectileDecorator()
    {
        if (statDecorators.Count > 0) RemoveDecoratorAtIndex(statDecorators, statDecorators.Count - 1, statProvider);
    }

    public void RemoveAll()
    {
        statDecorators.Clear();
        healthDecorators.Clear();
        resourceDecorators.Clear();
    }

    void RemoveDecoratorAtIndex<TDecorator, TProvider>(List<TDecorator> decorators, int index, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        if (index >= decorators.Count || index < 0)
        {
            Debug.LogError("Index out of range " + index + " " + decorators.Count);
            return;
        }

        if (index + 1 < decorators.Count && index - 1 >= 0) decorators[index + 1].provider = decorators[index - 1] as TProvider;
        else if (index == 0 && decorators.Count > 1) decorators[index + 1].provider = baseProvider;

        decorators.RemoveAt(index);
    }

    public BaseStatProvider GetCurrentStatProvider()
    {
        return statDecorators.Count > 0 ? statDecorators.Last() as BaseStatProvider : statProvider;
    }

    public BaseEntityHealthProvider GetCurrentHealthProvider()
    {
        return healthDecorators.Count > 0 ? healthDecorators.Last() as BaseEntityHealthProvider : healthProvider;
    }

    public BaseEntityResourceProvider GetCurrentResourceProvider()
    {
        return resourceDecorators.Count > 0 ? resourceDecorators.Last() as BaseEntityResourceProvider : resourceProvider;
    }
}
