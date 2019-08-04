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

    #region Projectile

    public void ApplyProjectileDecorator(int extra)
    {
        statDecorators.Add(new ExtraProjectilesDecorator(GetCurrentStatProvider(), extra));
    }

    public void RemoveProjectileDecorator()
    {
        if (statDecorators.Count > 0) RemoveDecoratorAtIndex(statDecorators, statDecorators.Count - 1, statProvider);
    }

    #endregion

    #region Remove

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

    void RemoveDecorator<TDecorator, TProvider>(List<TDecorator> decorators, TDecorator decorator, TProvider baseProvider)
        where TProvider : BaseProvider
        where TDecorator : IDecorator<TProvider>
    {
        RemoveDecoratorAtIndex(decorators, decorators.IndexOf(decorator), baseProvider);
    }

    #endregion

    public void ApplyItem(EquipItem item)
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

    void EvaluateStatDecorator(ConvertedStat stat, out IHealthDecorator hpDec, out IStatDecorator statDec)
    {
        var value = stat.value;
        bool add = stat.mathType == ModMathType.Additive;
        var p = GetCurrentStatProvider();
        var h = GetCurrentHealthProvider();

        hpDec = null;
        statDec = null;

        switch (stat.type)
        {
            case StatType.Health:

                if (add) healthDecorators.Add(hpDec = new HealthAddDecorator(h, (int)value));
                else healthDecorators.Add(hpDec = new HealthMultiDecorator(h, value));

                break;
            case StatType.ActionCooldown:
            case StatType.Resource:
            case StatType.Power:

                statDecorators.Add(statDec = new SingleStatDecorator(p, stat.type, stat.mathType, value));

                break;
            default:
                break;
        }
    }

    public void RemoveItem(EquipItem item)
    {
        foreach (var stat in item.statDecorators) RemoveDecorator(statDecorators, stat, statProvider);
        foreach (var hp in item.healthDecorators) RemoveDecorator(healthDecorators, hp, healthProvider);
    }

    #region Current

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

    #endregion
}
