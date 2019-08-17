using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroCollection : MonoSingleton<HeroCollection>
{
    public List<Hero> heroes = new List<Hero>();

    public void Select(Hero rep)
    {
        rep.state = HeroState.Selected;
    }

    public void DeSelect(Hero rep)
    {
        rep.state = HeroState.Recruited;
    }

    public void Recruit(Hero rep)
    {
        rep.state = HeroState.Recruited;
    }

    public void Remove(Hero rep)
    {
        heroes.Remove(rep);
    }

    public List<Hero> GenerateApplying(int num, bool clearCurrent = true)
    {
        if (clearCurrent)
        {
            var rec = GetApplying();
            foreach (var r in rec) heroes.Remove(r);
        }

        for (int i = 0; i < num; ++i)
        {
            var rep = HeroCreator.CreateRandomHero();
            rep.state = HeroState.Applying;
            heroes.Add(rep);
        }

        return GetApplying();
    }

    public List<Hero> GetApplying() => GetHeroes(HeroState.Applying);
    public List<Hero> GetRecruited(HeroSortMode sortMode) => GetHeroes(HeroState.Recruited, sortMode);
    public List<Hero> GetSelected() => GetHeroes(HeroState.Selected);

    List<Hero> GetHeroes(HeroState state, HeroSortMode sortMode = HeroSortMode.None)
    {
        var list = (from h in heroes where h.state == state select h).ToList();
        switch (sortMode)
        {
            case HeroSortMode.None:
                break;
            case HeroSortMode.ClassType:
                list = list.OrderBy(x => x.Prefab.name).ToList();
                break;
            case HeroSortMode.Level:
                list = list.OrderBy(x => x.Level).Reverse().ToList();
                break;
            default:
                break;
        }

        return list;
    }
}

public enum HeroSortMode
{
    None = 0,
    ClassType = 1,
    Level = 2
}
