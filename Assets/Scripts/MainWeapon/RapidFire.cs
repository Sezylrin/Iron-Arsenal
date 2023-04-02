using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : MonoBehaviour, ICannonProjectile
{
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

    private float fireDelay = 0.2f;
    public float FireDelay
    {
        get { return fireDelay; }
        set { fireDelay = value; }
    }

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
        Invoke("delete", 3f);

        newBullet = Instantiate(bullet, transform.position, transform.rotation, projectilesParent); //0
        bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.Direction = this.Direction;
        bulletScript.Damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void delete()
    {
        Destroy(gameObject);
    }
}
