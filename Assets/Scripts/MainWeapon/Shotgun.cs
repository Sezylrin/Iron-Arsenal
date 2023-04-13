using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour, ICannonProjectile
{
    public Cannon Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; } = 20f;
    public float ProjectileSpeed { get; set; } = 0f;
    public float FireDelay { get; set; } = 0.8f;

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

            if (angleAdjustment == 0)
            {
                newBullet.transform.rotation = transform.rotation;
            }
            else newBullet.transform.rotation = transform.rotation * new Quaternion(1, 0, angleAdjustment, 0);
            newBullet.transform.position = transform.position;

            bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.Direction = this.Direction;
            bulletScript.Damage = Damage;
            bulletScript.Owner = Owner;
            bulletScript.Shoot();

            angleAdjustment -= 5;
        }
        StopCoroutine(Delete(0f));
        StartCoroutine(Delete(0.5f));
    }

    public IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.PoolShotgun(gameObject);
    }
}
