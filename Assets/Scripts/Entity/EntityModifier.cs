using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityModifier
{
    BaseEntityHealthProvider healthProvider;
    BaseEntityResourceProvider resourceProvider;
    BaseStatProvider statProvider;
    bool allowHealth, allowResource, allowStat;

    List<List<IStatDecorator>> sortedStatDecorators = new List<List<IStatDecorator>>();
    List<List<IHealthDecorator>> sortedHealthDecorators = new List<List<IHealthDecorator>>();
    List<IResourceDecorator> resourceDecorators = new List<IResourceDecorator>();

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


    void RemoveDecorator<TDecorator, TProvider>(List<List<TDecorator>> decorators, TDecorator[] decoratorsToRemove, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        foreach(var sublist in decorators)
        {
            foreach (var decorator in decoratorsToRemove)
            {
                sublist.Remove(decorator);
            }
        }

        RebuildDecorators(decorators, baseProvider);
    }

    #endregion

    public void ApplyItem(EquipableItem item)
    {
        foreach (var mod in item.mods)
        {
            foreach(var stat in mod.GetUsedStats())
            {
                CreateDecoratorsFromStat(stat, out IHealthDecorator hpDec, out IStatDecorator statDec);
                if (hpDec != null) item.healthDecorators.Add(hpDec);
                if (statDec != null) item.statDecorators.Add(statDec);
            }
        }
    }

    void CreateDecoratorsFromStat(StatStruct stat, out IHealthDecorator hpDec, out IStatDecorator statDec)
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
            case StatType.Range:

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
                    AddDecoratorToSingle(decorators, 1, decorator, baseProvider);
                    break;
                default:
                    break;
            }
        }
        else if (decorator as HealthAddDecorator != null) AddDecoratorToSingle(decorators, 0, decorator, baseProvider);
        else if (decorator as HealthMultiDecorator != null) AddDecoratorToSingle(decorators, 1, decorator, baseProvider);
        else
        {
            AddDecoratorToSingle(decorators, 2, decorator, baseProvider);
        }
    }

    void AddDecoratorToSingle<TDecorator, TProvider>(List<List<TDecorator>> decorators, int subIndex, TDecorator decorator, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        decorators[subIndex].Add(decorator);
        RebuildDecorators(decorators, baseProvider);
    }

    void RebuildDecorators<TDecorator, TProvider>(List<List<TDecorator>> decorators, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        List<TDecorator> builtList = new List<TDecorator>();
        decorators.ForEach(x =>
        {
            x.ForEach(y => builtList.Add(y));
        });

        if (builtList.Count > 0) builtList[0].provider = baseProvider;

        for (int i = 0; i < builtList.Count - 1; i++) builtList[i + 1].provider = builtList[i] as TProvider;
    }

    public void AddDecorator(IStatDecorator dec, ModMathType mathtype)
    {
        AddDecoratorSorted(sortedStatDecorators, dec, statProvider, mathtype);
    }
    public void AddDecorator(IHealthDecorator dec)
    {
        AddDecoratorSorted(sortedHealthDecorators, dec, healthProvider, ModMathType.Additive);
    }
    public void RemoveDecorator(IStatDecorator dec)
    {
        RemoveDecorator(sortedStatDecorators, new[] { dec }, statProvider);
    }
    public void RemoveDecorator(IHealthDecorator dec)
    {
        RemoveDecorator(sortedHealthDecorators, new[] { dec }, healthProvider);
    }

    public void RemoveItem(EquipableItem item)
    {
        RemoveDecorator(sortedStatDecorators, item.statDecorators.ToArray(), statProvider);
        RemoveDecorator(sortedHealthDecorators, item.healthDecorators.ToArray(), healthProvider);
        item.statDecorators.Clear();
        item.healthDecorators.Clear();
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
        return resourceDecorators.Count > 0 ? resourceDecorators.Last() as IEntityResourceProvider : resourceProvider;
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
        //foreach(var list in sortedHealthDecorators)
        //{
        //    foreach(var dec in list)
        //    {
        //        Debug.Log(dec);
        //        Debug.Log(dec.provider.GetCurrent());
        //        Debug.Log(dec.provider.GetMax());
        //        Debug.Log(dec.provider.GetPercentage());
        //    }
        //}

        foreach(var list in sortedHealthDecorators)
        {
            for (int i = 0; i < list.Count - 1; ++i)
            {
                if (list[i].provider != healthProvider && list[i + 1].provider != list[i] as BaseProvider)
                {
                    Debug.Log(i);
                }
            }
        }
    }

#endif

    #endregion
}
