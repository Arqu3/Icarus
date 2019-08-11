using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Damage, speed")]
    [SerializeField]
    protected float speed = 50f;

    protected int damage = 15;
    protected DamageType damageType = DamageType.Magical;
    protected EntityType ownerType = EntityType.Neutral;

    protected Rigidbody body;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    protected virtual IEnumerator Start()
    {
        yield return null;
        body.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public virtual void Initialize(DamageType dType, int damage, EntityType ownerType)
    {
        this.ownerType = ownerType;
        this.damage = damage;
        damageType = dType;
    }

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<ICombatEntity>();
        if (entity != null)
        {
            if (entity.EntityType == ownerType) return;
            OnHitEntity(entity);
        }
    }

    protected virtual void OnHitEntity(ICombatEntity entity)
    {
        entity.RemoveHealth(damage);
        Destroy(gameObject);
    }
}
