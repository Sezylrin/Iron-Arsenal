using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : MonoBehaviour, ICannonProjectile
{
    public Cannon Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; } = 8f;
    public float ProjectileSpeed { get; set; } = 0f;
    public float FireDelay { get; set; } = 0.2f;

    public GameObject bullet;
    private GameObject newBullet;
    private Bullet bulletScript;
    private Transform projectilesParent;

    void Awake()
    {
        projectilesParent = GameObject.Find("Projectiles").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if (Owner.pools[0].ListCount() > 0)
        {
            newBullet = Owner.pools[0].FirstObj();
            newBullet.SetActive(true);
            Owner.pools[0].RemoveObj(newBullet);
        }
        else
        {
            newBullet = Instantiate(bullet, projectilesParent);
        }
        newBullet.transform.rotation = transform.rotation;
        newBullet.transform.position = transform.position;

        bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.Direction = this.Direction;
        bulletScript.Damage = Damage;
        bulletScript.Owner = Owner;
        bulletScript.Shoot();

        StopCoroutine(Delete(0f));
        StartCoroutine(Delete(0.5f));
    }

    public IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.PoolRapidFire(gameObject);
    }
}
