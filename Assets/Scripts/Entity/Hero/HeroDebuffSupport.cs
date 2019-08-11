using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDebuffSupport : HeroSupport
{
    protected override IActionProvider CreateActionProvider()
    {
        return new DebuffSupportActionProvider(this);
    }
}
