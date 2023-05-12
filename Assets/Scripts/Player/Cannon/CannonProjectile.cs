using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonProjectileType
{
    Bullet,
    Shotgun,
    RapidFire,
    SlowShot,
    PoisonShot,
    Rocket,
    Flame
};

public abstract class CannonProjectile : MonoBehaviour
{
    public CannonProjectileType type;

    public StatAttribute attribute;

    public GameObject ProjectilePF;
    public Cannon Owner { get; set; }

    public Vector3 mousePos;

    public ProjectileData data;

    public float delay;

    public Pooling projectiles = new Pooling();

    public List<Augments> activeAugments = new List<Augments>();

    public Material cannonMat;
    
    public virtual void PoolProj(GameObject obj)
    {
        projectiles.AddObj(obj);
        OnDelete();
    }

    public virtual void Shoot(Vector3 dir)
    {
        GameObject bullet;
        Projectile bulletProj;
        if (projectiles.ListCount() > 0)
        {
            bullet = projectiles.FirstObj();
            bullet.SetActive(true);
            projectiles.RemoveObj(bullet);
        }
        else
        {
            bullet = Instantiate(ProjectilePF, Vector3.zero, Quaternion.identity, transform); 
        }
        bulletProj = bullet.GetComponent<Projectile>();
        bulletProj.SetProjectileData(data, this, attribute);
        if (activeAugments.Count - bulletProj.activeAugments.Count == 1)
        {
            Augments augmentToAdd = activeAugments[activeAugments.Count - 1];
            bulletProj.activeAugments.Add(augmentToAdd);
            AugmentBase temp = AugmentManager.Instance.AddAugmentToProjectile(augmentToAdd, bullet, bulletProj);
            if (temp)
            {
                bulletProj.augmentBases.Add(temp);
                temp.Init();
            }
        }
        else if (activeAugments.Count - bulletProj.activeAugments.Count > 1)
        {
            AddAllAugments(bullet, bulletProj);
        }
        foreach (AugmentBase augmentBase in bulletProj.augmentBases)
        {
            augmentBase.Respawn();
        }
        Vector3 spawn = Owner.cannonProjectileSpawnPoint.position;
        bulletProj.setSpawn(spawn);
        Vector3 targetDir = dir;
        targetDir.y = 0;
        bulletProj.dir = targetDir.normalized;
        bulletProj.SetRotation(dir,Vector3.zero);
    }

    public void AddAllAugments(GameObject bullet, Projectile projData)
    {
        foreach (Augments augmentToAdd in activeAugments)
        {
            if (!projData.activeAugments.Contains(augmentToAdd))
            {
                AugmentBase temp = AugmentManager.Instance.AddAugmentToProjectile(augmentToAdd, bullet, projData);
                if (temp)
                {
                    projData.augmentBases.Add(temp);
                    temp.Init();
                }
                projData.activeAugments.Add(augmentToAdd);
            }
        }
    }

    public void AddAugments(Augments augmentToAdd)
    {
        if (!activeAugments.Contains(augmentToAdd))
            activeAugments.Add(augmentToAdd);
    }
    public virtual void SetStats(Cannon owner, Vector3 mousePos)
    {
        Owner = owner;
        this.mousePos = mousePos;
    }

    public virtual void Init()
    {
        Invoke("StartDelay", delay);
    }

    public virtual void StartDelay()
    {
        Owner.DelayFiring();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            DeleteNow();
        }

        if (other.gameObject.tag == "Wall")
        {
            DeleteNow();
        }*/
    }



    public virtual void OnDelete()
    {
        switch (type)
        {
            case CannonProjectileType.Bullet:
                Owner.PoolProjectile(gameObject, 0);
                break;
            case CannonProjectileType.Shotgun:
                Owner.PoolProjectile(gameObject, 1);
                break;
            case CannonProjectileType.RapidFire:
                Owner.PoolProjectile(gameObject, 2);
                break;
            case CannonProjectileType.SlowShot:
                Owner.PoolProjectile(gameObject, 3);
                break;
            case CannonProjectileType.PoisonShot:
                Owner.PoolProjectile(gameObject, 4);
                break;
            case CannonProjectileType.Rocket:
                Owner.PoolProjectile(gameObject, 5);
                break;
            case CannonProjectileType.Flame:
                Owner.PoolProjectile(gameObject, 6);
                break;
        }
    }
}
