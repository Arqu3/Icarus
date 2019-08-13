using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroCollection : MonoSingleton<HeroCollection>
{
    public List<Hero> representations = new List<Hero>();

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
        representations.Remove(rep);
    }

    public List<Hero> GenerateApplying(int num, bool clearCurrent = true)
    {
        if (clearCurrent)
        {
            var rec = GetApplying();
            foreach (var r in rec) representations.Remove(r);
        }

        for (int i = 0; i < num; ++i)
        {
            var rep = HeroCreator.CreateRandomHero();
            rep.state = HeroState.Applying;
            representations.Add(rep);
        }

        return GetApplying();
    }

    public List<Hero> GetApplying() => GetRepresentations(HeroState.Applying);
    public List<Hero> GetRecruited() => GetRepresentations(HeroState.Recruited);
    public List<Hero> GetSelected() => GetRepresentations(HeroState.Selected);

    List<Hero> GetRepresentations(HeroState state)
    {
        return (from h in representations where h.state == state select h).ToList();
    }
}
