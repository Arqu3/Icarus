using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSupport : HeroEntity
{
    protected override IActionProvider CreateActionProvider()
    {
        return new SupportActionProvider(this);
    }
}
