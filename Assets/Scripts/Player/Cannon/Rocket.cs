using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, ICannonProjectile
{
    public Cannon Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; } = 25f;
    public float ProjectileSpeed { get; set; } = 20f;
    public float FireDelay { get; set; } = 0.5f;

    public CannonProjectileData data;

    public GameObject explosion;

    void Awake()
    {
        Damage = data.damage;
        ProjectileSpeed = data.projectileSpeed;
        FireDelay = data.fireDelay;
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
        StartCoroutine(Delete(2f));
    }

    public IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosion, new Vector3(transform.position.x, -2, transform.position.z), transform.rotation);
        Owner.PoolProjectile(gameObject, 5);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<IEnemy>().TakeDamage(Damage);
            StartCoroutine(Delete(0f));
        }
    }
}
