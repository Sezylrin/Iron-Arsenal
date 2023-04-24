using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBossProjectile : MonoBehaviour
{
    public Boss Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float FireDelay { get; set; }
    public float ProjectileLifetime { get; set; }

    public EnemyBulletData data;

    protected virtual void Init()
    {
        SetStats();
    }

    public virtual void SetStats()
    {
        Damage = data.damage * Mathf.Pow(1.1f, Owner.Wave);
        ProjectileSpeed = data.projectileSpeed * Mathf.Pow(1.01f, Owner.Wave);
        FireDelay = data.fireDelay * Mathf.Pow(1.01f, -Owner.Wave);
        ProjectileLifetime = data.projectileLifetime;
    }

    public virtual void SetStats(Boss owner, Vector3 direction, float damage, float projectileSpeed, float fireDelay)
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
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BaseFunctions>().TakeDamage(Damage);
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
        Destroy(gameObject);
    }

    public virtual void DeleteNow()
    {
        OnDelete();
    }
}
