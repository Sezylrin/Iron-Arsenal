using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : CannonProjectile
{
    private GameObject newBullet;
    private Bullet bulletScript;
    private Transform projectilesParent;
    private float angleAdjustment;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public override void Shoot()
    {
        angleAdjustment = 8f;
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
            bulletScript.SetStats(Owner, Direction, Damage, ProjectileSpeed, FireDelay);
            bulletScript.Shoot();

            angleAdjustment -= 4;
        }
        base.Shoot();
    }*/

    public override void Init()
    {
        angleAdjustment = 8f;
        for (int i = 0; i < 5; i++)
        {
            Vector3 tempPos = transform.position;
            tempPos.y = 0;
            Vector3 tempMouse = mousePos;
            tempMouse.y = 0;
            Vector3 dir = tempMouse - tempPos;
            dir = Quaternion.AngleAxis(angleAdjustment, Vector3.up) * dir;
            Shoot(dir);
            angleAdjustment -= 4;
        }
        base.Init();
    }

    public override void PoolProj(GameObject obj)
    {
        projectiles.AddObj(obj);
        if (projectiles.ListCount() == 5)
            OnDelete();
    }
}
