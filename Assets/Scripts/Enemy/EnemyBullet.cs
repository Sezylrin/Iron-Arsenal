using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Enemy Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float FireDelay { get; set; }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction.x * ProjectileSpeed * Time.deltaTime, 0, Direction.z * ProjectileSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        StopCoroutine(Delete(0f));
        StartCoroutine(Delete(10f));
    }

    public IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.Manager.PoolEnemyBullet(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BaseFunctions>().TakeDamage(Damage);
            StartCoroutine(Delete(0f));
        }
    }

    public void SetStats(float damage, float projectileSpeed, float fireDelay)
    {
        Damage = damage;
        ProjectileSpeed = projectileSpeed;
        FireDelay = fireDelay;
    }
}
