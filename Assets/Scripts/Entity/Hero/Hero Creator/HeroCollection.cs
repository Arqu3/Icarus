using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroCollection : MonoSingleton<HeroCollection>
{
    public List<HeroRepresentation> representations = new List<HeroRepresentation>();

    public void Select(HeroRepresentation rep)
    {
        rep.repState = HeroRepState.Selected;
    }

    public void DeSelect(HeroRepresentation rep)
    {
        rep.repState = HeroRepState.Recruited;
    }

    public void Recruit(HeroRepresentation rep)
    {
        rep.repState = HeroRepState.Recruited;
    }

    public void Remove(HeroRepresentation rep)
    {
        representations.Remove(rep);
    }

    public List<HeroRepresentation> GenerateApplying(int num, bool clearCurrent = true)
    {
        if (clearCurrent)
        {
            var rec = GetApplying();
            foreach (var r in rec) representations.Remove(r);
        }

        for (int i = 0; i < num; ++i)
        {
            var rep = HeroCreator.CreateRandomRepresentation();
            rep.repState = HeroRepState.Applying;
            representations.Add(rep);
        }

        return GetApplying();
    }

    public List<HeroRepresentation> GetApplying() => GetRepresentations(HeroRepState.Applying);
    public List<HeroRepresentation> GetRecruited() => GetRepresentations(HeroRepState.Recruited);
    public List<HeroRepresentation> GetSelected() => GetRepresentations(HeroRepState.Selected);

    List<HeroRepresentation> GetRepresentations(HeroRepState state)
    {
        return (from h in representations where h.repState == state select h).ToList();
    }
}
