using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[RequireComponent(typeof(ParticleSystem))]
public class LightningProjectile : Projectile
{
    [SerializeField]
    int numBounces = 1;
    [SerializeField]
    float bounceRadius = 5f;

    ParticleSystem system;
    List<ICombatEntity> hits = new List<ICombatEntity>();

    const float distanceTime = 0.014f;

    protected override void Awake()
    {
        base.Awake();
        system = GetComponent<ParticleSystem>();
    }

    protected override IEnumerator Start()
    {
        yield return null;

        StartCoroutine(_Bounce());
    }

    IEnumerator _Bounce()
    {
        ICombatEntity[] targets = (from t in TargetProvider.Get() where t.EntityType != ownerType select t).ToArray();

        if (targets.Length <= 0)
        {
            Destroy(gameObject);
            yield break;
        }
        Vector3 previousPosition = transform.position;

        system.Emit(1);
        var target = targets.Random();
        transform.position = target.transform.position;

        yield return new WaitForSeconds(distanceTime * Vector3.Distance(transform.position, previousPosition));

        if (target == null || !target.Valid)
        {
            Destroy(gameObject);
            yield break;
        }
        DealDamage(target);

        for (int i = 0; i < numBounces; ++i)
        {
            var available = (from h
                in Physics.OverlapSphere(transform.position, bounceRadius)
                             let e = h.GetComponent<ICombatEntity>()
                             where e != null && e.EntityType != ownerType && !hits.Contains(e)
                             select e).ToArray();

            if (available.Length <= 0) break;
            previousPosition = transform.position;

            target = available.Random();
            transform.position = target.transform.position;
            yield return new WaitForSeconds(distanceTime * Vector3.Distance(transform.position, previousPosition));
            if (target == null || !target.Valid) break;
            DealDamage(target);
        }

        yield return new WaitForSeconds(distanceTime * Vector3.Distance(transform.position, previousPosition));
        Destroy(gameObject);
    }

    void DealDamage(ICombatEntity entity)
    {
        entity.RemoveHealth(damage, damageType);
        hits.Add(entity);
    }

    protected override void OnHitEntity(ICombatEntity entity)
    {
    }
}
