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
    public List<Hero> GetRecruited() => GetHeroes(HeroState.Recruited);
    public List<Hero> GetSelected() => GetHeroes(HeroState.Selected);

    List<Hero> GetHeroes(HeroState state)
    {
        return (from h in heroes where h.state == state select h).ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach(var h in GetRecruited())
            {
                Debug.Log(h.Prefab.name);
                Debug.Log(h.Items.Count);
            }
        }
    }
}
