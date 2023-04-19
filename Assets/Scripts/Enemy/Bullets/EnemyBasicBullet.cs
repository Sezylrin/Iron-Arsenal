using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicBullet : MonoBehaviour
{
    public EnemyManager Manager { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float FireDelay { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction.x * ProjectileSpeed * Time.deltaTime, 0, Direction.z * ProjectileSpeed * Time.deltaTime);
    }

    public virtual void SetStats(float damage, float projectileSpeed, float fireDelay)
    {
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        FireDelay = fireDelay;
    }

    public virtual void SetStats(EnemyManager manager, Vector3 direction, float damage, float projectileSpeed, float fireDelay)
    {
        Manager = manager;
        Direction = direction;
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        FireDelay = fireDelay;
    }

    public void Shoot()
    {
        StopCoroutine(Delete(0f));
        StartCoroutine(Delete(3f));
    }

    protected IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Manager.PoolEnemyBullet(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<tempPlayer>().TakeDamage(Damage);
            StartCoroutine(Delete(0f));
        }
    }
}
