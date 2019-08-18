using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebuffSupportActionProvider : SupportActionProvider
{
    public DebuffSupportActionProvider(ICombatEntity owner, DamageType damageType) : base(owner, damageType)
    {
    }

    List<ICombatEntity> debuffedEntities = new List<ICombatEntity>();

    public override void Update()
    {
        if (!HasTarget) Target = LookForRandomEnemyTarget();
        else
        {
            UpdateMovement();

            if (IsInRange && !IsOnCooldown)
            {
                debuffedEntities.RemoveAll(x => x == null || !x.Valid);

                if (debuffedEntities.Count > 0 && owner.SpendResourcePercentage(SpecialResourcePercentageCost)) PerformSpecial();
                else
                {
                    if (debuffedEntities.Contains(Target)) Target = GetNonDebuffedEntity();
                    if (HasTarget) PerformBasic();
                }
            }
        }
    }

    ICombatEntity GetNonDebuffedEntity()
    {
        var c = (
            from e
            in GetEnemyEntities()
            where !debuffedEntities.Contains(e)
            select e).ToArray();

        if (c.Length > 0) return c.Random();
        else return null;
    }


    protected override void PerformBasic()
    {
        DebuffEntity(Target, 3f, new SingleStatDecorator(null, StatType.Power, ModMathType.Multiplicative, 0.8f));
        owner.GiveResource(CurrentStatProvider.GetResourceGain());

        StartCooldown();
    }

    protected override void PerformSpecial()
    {
        foreach(var entity in debuffedEntities)
        {
            entity.StartCoroutine(_DebuffEntityNoList(entity, 3f, new SingleStatDecorator(null, StatType.ActionCooldown, ModMathType.Multiplicative, 0.4f * CurrentStatProvider.GetPower())));
        }

        StartCooldown();
    }

    IEnumerator _DebuffEntityNoList(ICombatEntity entity, float duration, SingleStatDecorator debuff)
    {
        entity.GetModifier().AddDecorator(debuff, debuff.MathType);

        yield return new WaitForSeconds(duration);

        entity.GetModifier().RemoveDecorator(debuff);
    }

    Coroutine DebuffEntity(ICombatEntity entity, float duration, SingleStatDecorator debuff)
    {
        if (debuffedEntities.Contains(entity)) return null;

        return entity.StartCoroutine(_DebuffEntity(entity, duration, debuff));
    }

    IEnumerator _DebuffEntity(ICombatEntity entity, float duration, SingleStatDecorator debuff)
    {
        debuffedEntities.Add(entity);
        entity.GetModifier().AddDecorator(debuff, debuff.MathType);

        yield return new WaitForSeconds(duration);

        entity.GetModifier().RemoveDecorator(debuff);
        debuffedEntities.Remove(entity);
    }
}
