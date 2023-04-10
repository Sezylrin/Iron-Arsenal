using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : CannonProjectile
{
    public GameObject bullet;
    private GameObject newBullet;
    private Bullet bulletScript;
    private Transform projectilesParent;

    void Awake()
    {
        Init();
        projectilesParent = GameObject.Find("Projectiles Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
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
        bulletScript.SetStats(Owner, Direction, Damage, ProjectileSpeed, FireDelay);
        bulletScript.Shoot();

        base.Shoot();
    }
}
