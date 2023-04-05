using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour, ICannonProjectile
{
    private Cannon owner;
    public Cannon Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    private Vector3 direction;
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private float damage = 8f;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private float speed = 0f;
    public float ProjectileSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    private float fireDelay = 0.8f;
    public float FireDelay
    {
        get { return fireDelay; }
        set { fireDelay = value; }
    }

    public GameObject bullet;
    private GameObject newBullet;
    private Bullet bulletScript;
    private Transform projectilesParent;
    private float angleAdjustment;

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
        angleAdjustment = 10f;
        for (int i = 0; i < 5; i++)
        {
            if (owner.pools[0].ListCount() > 0)
            {
                newBullet = owner.pools[0].FirstObj();
                newBullet.SetActive(true);
                owner.pools[0].RemoveObj(newBullet);
            }
            else
            {
                newBullet = Instantiate(bullet, projectilesParent);
            }


            bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.Direction = this.Direction;
            bulletScript.Damage = damage;
            bulletScript.Owner = owner;
            if (angleAdjustment == 0)
            {
                newBullet.transform.rotation = transform.rotation;
            }
            else newBullet.transform.rotation = transform.rotation * new Quaternion(1, 0, angleAdjustment, 0);
            newBullet.transform.position = transform.position;
            bulletScript.Shoot();
            angleAdjustment -= 5;
        }
        Invoke("Delete", 1f);
    }

    public void Delete()
    {
        Owner.PoolShotgun(gameObject);
    }
}