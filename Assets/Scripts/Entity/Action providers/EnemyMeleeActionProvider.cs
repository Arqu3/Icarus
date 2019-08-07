using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeActionProvider : MeleeActionProvider
{
    public EnemyMeleeActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    public override IStatProvider CreateBaseStatProvider()
    {
        var data = EnemyMeleeData.Instance;
        return new DefaultStatProvider(data.Power, data.ResourceGain, data.ActionCooldown, data.Range);
    }
}
