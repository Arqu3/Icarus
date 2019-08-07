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

    List<List<IStatDecorator>> sortedStatDecorators = new List<List<IStatDecorator>>();
    List<List<IHealthDecorator>> sortedHealthDecorators = new List<List<IHealthDecorator>>();

    //List<IStatDecorator> statDecorators = new List<IStatDecorator>();
    List<IResourceDecorator> resourceDecorators = new List<IResourceDecorator>();
    //List<IHealthDecorator> healthDecorators = new List<IHealthDecorator>();

    public EntityModifier(IEntityHealthProvider healthProvider, IEntityResourceProvider resourceProvider, IStatProvider statProvider)
    {
        this.statProvider = statProvider as BaseStatProvider;
        this.healthProvider = healthProvider as BaseEntityHealthProvider;
        this.resourceProvider = resourceProvider as BaseEntityResourceProvider;

        for (int i = 0; i < 3; ++i)
        {
            sortedStatDecorators.Add(new List<IStatDecorator>());
            sortedHealthDecorators.Add(new List<IHealthDecorator>());
        }

        allowHealth = healthProvider != null;
        allowResource = resourceProvider != null;
        allowStat = statProvider != null;
    }

    #region Remove

    public void RemoveAll()
    {
        sortedStatDecorators.ForEach(x => x.Clear());
        sortedHealthDecorators.ForEach(x => x.Clear());
        resourceDecorators.Clear();
    }

    //void RemoveDecoratorAtIndex<TDecorator, TProvider>(List<TDecorator> decorators, int index, TProvider baseProvider)
    //    where TProvider : BaseProvider
    //    where TDecorator : IDecorator<TProvider>
    //{
    //    if (index >= decorators.Count || index < 0)
    //    {
    //        Debug.LogError("Index out of range " + index + " " + decorators.Count);
    //        return;
    //    }

    //    if (index + 1 < decorators.Count && index - 1 >= 0) decorators[index + 1].provider = decorators[index - 1] as TProvider;
    //    else if (index == 0 && decorators.Count > 1) decorators[index + 1].provider = baseProvider;

    //    decorators.RemoveAt(index);
    //}


    void RemoveDecorator<TDecorator, TProvider>(List<List<TDecorator>> decorators, TDecorator decorator, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        foreach(var sublist in decorators)
        {
            if (sublist.Contains(decorator))
            {
                sublist.Remove(decorator);
                break;
            }
        }

        List<TDecorator> builtList = new List<TDecorator>();
        decorators.ForEach(x =>
        {
            x.ForEach(y => builtList.Add(y));
        });

        if (builtList.Count > 0) builtList[0].provider = baseProvider;

        for (int i = 0; i < builtList.Count - 1; i++) builtList[i + 1].provider = builtList[i] as TProvider;
    }

    #endregion

    public void ApplyItem(EquipableItem item)
    {
        foreach (var mod in item.mods)
        {
            foreach(var stat in mod.GetUsedStats())
            {
                EvaluateStatDecorator(stat, out IHealthDecorator hpDec, out IStatDecorator statDec);
                if (hpDec != null) item.healthDecorators.Add(hpDec);
                if (statDec != null) item.statDecorators.Add(statDec);
            }
        }
    }

    void EvaluateStatDecorator(StatStruct stat, out IHealthDecorator hpDec, out IStatDecorator statDec)
    {
        var value = stat.value;
        bool add = stat.mathType == ModMathType.Additive;

        hpDec = null;
        statDec = null;

        switch (stat.type)
        {
            case StatType.Health:

                if (add) hpDec = new HealthAddDecorator(null, (int)value);
                else hpDec = new HealthMultiDecorator(null, value);
                AddDecoratorSorted(sortedHealthDecorators, hpDec, healthProvider, ModMathType.Additive);

                break;
            case StatType.ActionCooldown:
            case StatType.Resource:
            case StatType.Power:

                statDec = new SingleStatDecorator(null, stat.type, stat.mathType, value);
                AddDecoratorSorted(sortedStatDecorators, statDec, statProvider, stat.mathType);

                break;
            default:
                break;
        }
    }

    //Additive - multiplicative - other
    void AddDecoratorSorted<TDecorator, TProvider>(List<List<TDecorator>> decorators, TDecorator decorator, TProvider baseProvider, ModMathType mathtype)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        if (decorator as SingleStatDecorator != null)
        {
            switch (mathtype)
            {
                case ModMathType.Additive:
                    AddDecoratorToSingle(decorators, 0, decorator, baseProvider);
                    break;
                case ModMathType.Multiplicative:
                    AddDecoratorToSingle(decorators, 1, decorator, decorators[0].Count > 0 ? decorators[0].Last() as TProvider : baseProvider);
                    break;
                default:
                    break;
            }
        }
        else if (decorator as HealthAddDecorator != null) AddDecoratorToSingle(decorators, 0, decorator, baseProvider);
        else if (decorator as HealthMultiDecorator != null) AddDecoratorToSingle(decorators, 1, decorator, decorators[0].Count > 0 ? decorators[0].Last() as TProvider : baseProvider);
        else
        {
            Debug.Log(typeof(TDecorator));
            AddDecoratorToSingle(decorators, 2, decorator, decorators[1].Count > 0 ? decorators[1].Last() as TProvider : baseProvider);
        }
    }

    void AddDecoratorToSingle<TDecorator, TProvider>(List<List<TDecorator>> decorators, int subIndex, TDecorator decorator, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        decorator.provider = decorators[subIndex].Count > 0 ? decorators[subIndex].Last() as TProvider : baseProvider;
        decorators[subIndex].Add(decorator);
        if (subIndex != decorators.Count - 1)
        {
            if (decorators[subIndex + 1].Count > 0) decorators[subIndex + 1].First().provider = decorator as TProvider;
        }
    }

    public void RemoveItem(EquipableItem item)
    {
        foreach (var stat in item.statDecorators) RemoveDecorator(sortedStatDecorators, stat, statProvider);
        foreach (var hp in item.healthDecorators) RemoveDecorator(sortedHealthDecorators, hp, healthProvider);
    }

    #region Current

    public IStatProvider GetCurrentStatProvider()
    {
        return GetProvider(sortedStatDecorators, statProvider);
    }

    public IEntityHealthProvider GetCurrentHealthProvider()
    {
        return GetProvider(sortedHealthDecorators, healthProvider);
    }

    public IEntityResourceProvider GetCurrentResourceProvider()
    {
        return resourceDecorators.Count > 0 ? resourceDecorators.Last() as BaseEntityResourceProvider : resourceProvider;
    }

    TProvider GetProvider<TDecorator, TProvider>(List<List<TDecorator>> list, TProvider baseProvider) 
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        if (list.Select(x => x.Count).Sum() > 0) return (from s in list where s.Count > 0 select s).Last().Last() as TProvider;
        else return baseProvider;
    }

    #endregion

    #region Debug

#if UNITY_EDITOR

    public void OutputHealthDecorators()
    {
        foreach(var list in sortedHealthDecorators)
        {
            foreach(var dec in list)
            {
                Debug.Log(dec);
                Debug.Log(dec.provider.GetCurrent());
                Debug.Log(dec.provider.GetMax());
                Debug.Log(dec.provider.GetPercentage());
            }
        }
    }

#endif

    #endregion
}
