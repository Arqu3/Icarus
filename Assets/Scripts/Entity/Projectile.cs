using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage, speed")]
    [SerializeField]
    float speed = 50f;

    int damage = 15;
    DamageType damageType = DamageType.Magical;
    EntityType ownerType = EntityType.Neutral;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void Initialize(DamageType dType, int damage, EntityType ownerType)
    {
        this.ownerType = ownerType;
        this.damage = damage;
        damageType = dType;
    }

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<BaseEntity>();
        if (entity)
        {
            if (entity.EntityType == ownerType) return;
            entity.RemoveHealth(damage);
            Destroy(gameObject);
        }
    }
}
