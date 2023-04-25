using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonProjectileType
{
    Bullet,
    Shotgun,
    RapidFire,
    SlowShot,
    PoisonShot,
    Rocket,
    Flame
};

public abstract class CannonProjectile : MonoBehaviour
{
    public CannonProjectileType type;

    public Cannon Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float FireDelay { get; set; }
    public float ProjectileLifetime { get; set; }

    public CannonProjectileData data;
    
    protected virtual void Init()
    {
        SetStats();
    }

    public virtual void SetStats()
    {
        Damage = data.damage * data.level;
        ProjectileSpeed = data.projectileSpeed + data.level;
        FireDelay = data.fireDelay - (0.05f * data.level);
        ProjectileLifetime = data.projectileLifetime;
    }

    public virtual void SetStats(Cannon owner, Vector3 direction, float damage, float projectileSpeed, float fireDelay)
    {
        Owner = owner;
        Direction = direction;
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        FireDelay = fireDelay;
        ProjectileLifetime = data.projectileLifetime;
    }

    protected virtual void Move()
    {
        transform.Translate(Direction.x * ProjectileSpeed * Time.deltaTime, 0, Direction.z * ProjectileSpeed * Time.deltaTime);
    }

    public virtual void Shoot()
    {
        StopCoroutine(StartDeletion(ProjectileLifetime));
        StartCoroutine(StartDeletion(ProjectileLifetime));
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            DeleteNow();
        }

        if (other.gameObject.tag == "Wall")
        {
            DeleteNow();
        }
    }

    public virtual IEnumerator StartDeletion(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnDelete();
    }

    public virtual void OnDelete()
    {
        switch (type)
        {
            case CannonProjectileType.Bullet:
                Owner.PoolProjectile(gameObject, 0);
                break;
            case CannonProjectileType.Shotgun:
                Owner.PoolProjectile(gameObject, 1);
                break;
            case CannonProjectileType.RapidFire:
                Owner.PoolProjectile(gameObject, 2);
                break;
            case CannonProjectileType.SlowShot:
                Owner.PoolProjectile(gameObject, 3);
                break;
            case CannonProjectileType.PoisonShot:
                Owner.PoolProjectile(gameObject, 4);
                break;
            case CannonProjectileType.Rocket:
                Owner.PoolProjectile(gameObject, 5);
                break;
            case CannonProjectileType.Flame:
                Owner.PoolProjectile(gameObject, 6);
                break;
        }
    }

    public virtual void DeleteNow()
    {
        OnDelete();
    }
}
