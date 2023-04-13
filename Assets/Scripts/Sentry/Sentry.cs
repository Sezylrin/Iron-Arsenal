using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform centerPoint;

    private Vector3 forwardDir;

    public float losAngle;

    public float range;

    public float fireRate;

    public Transform target;

    private List<Transform> tempList = new List<Transform>();

    public Transform testTransform;

    public GameObject projectilePF;

    private float timer;

    public Transform bulletSpawnpoint;

    private Pooling pooledBullets = new Pooling();

    public Transform sentryHead;

    public SentryData data;

    public GameObject bulletPool;

    private List<Augments> activeAugments;
    void Start()
    {
        bulletPool = new GameObject();
        tempList.Add(testTransform);
        if (!centerPoint)
            centerPoint = GameObject.FindWithTag("CenterPoint").transform;
        if (!centerPoint)
        {
            Debug.Log("No centerPoint for turrets to reference, please create an emepty transform at the center of the base");
            Debug.Break();
        }
        if (data)
            SetValue();
        if (AugmentManager.Instance)
        {
            AugmentManager.Instance.activeSentries.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!data)
            return;
        SetForward();
        if (!target)
        {
            LocateTarget(LevelManager.Instance.enemyManager.enemyList);
        }
        else
        {
            if (timer <= 0)
            {
                ShootTarget();
                timer = 1 / fireRate;
            }
            TargetCheck(target);
        }
        timer -= Time.deltaTime;
    }

    private void SetForward()
    {
        Vector3 tempCurrent = transform.position;
        tempCurrent.y = 0;
        Vector3 tempCenter = centerPoint.position;
        tempCenter.y = 0;
        forwardDir = tempCurrent - tempCenter;
        forwardDir.Normalize();
    }

    public void LocateTarget(List<Transform> targets)
    {
        foreach (Transform target in targets)
        {
            TargetCheck(target);
            if (this.target)
                return;
        }
    }

    private void ShootTarget()
    {
        GameObject bullet;
        if (pooledBullets.ListCount() > 0)
        {
            //Debug.Log("Grabbed from pool");
            bullet = pooledBullets.FirstObj();
            bullet.SetActive(true);
            pooledBullets.RemoveObj(bullet);
        }
        else
        {
            //Debug.Log("Spawned new");
            bullet = Instantiate(projectilePF, Vector3.zero, Quaternion.identity,bulletPool.transform);
        }
        Projectile bulletProj = bullet.GetComponent<Projectile>();
        if (activeAugments.Count - bulletProj.activeAugments.Count == 1)
        {
            Augments augmentToAdd = activeAugments[activeAugments.Count - 1];
            AugmentManager.Instance.AddAugmentToProjectile(augmentToAdd , bullet, bulletProj);
            bulletProj.activeAugments.Add(augmentToAdd);
        }
        else if (bulletProj.activeAugments.Count == 0 && activeAugments.Count != 0)
        {
            AddActiveAugment(bullet, bulletProj);
        }
        bulletProj.SetProjectileData(data.projectileData, this);
        bulletProj.setSpawn(bulletSpawnpoint.position);
        bulletProj.respawned = true;
        Vector3 targetDir = target.position - bulletSpawnpoint.position;
        targetDir.y = 0;
        bulletProj.SetDirection(targetDir);
        Vector3 lookAt = target.position;
        lookAt.y = sentryHead.position.y;
        sentryHead.LookAt(lookAt, Vector3.up);
    }


    public void PoolBullet(GameObject obj)
    {
        //Debug.Log("Sent to pool");
        obj.SetActive(false);
        pooledBullets.AddObj(obj);
    }

    private void TargetCheck(Transform target)
    {
        Vector3 tempCurrent = transform.position;
        tempCurrent.y = 0;
        Vector3 tempTarget = target.position;
        tempTarget.y = 0;
        Vector3 enemyDir = tempTarget - tempCurrent;
        float angle = Vector3.Angle(enemyDir, forwardDir);
        if (angle <= losAngle * 0.5 && Vector3.Distance(transform.position, target.position) <= range)
        {
            this.target = target;
        }
        else
        {
            this.target = null;
        }
    }

    public void RemoveTarget()
    {
        target = null;
    }

    private void SetValue()
    {
        losAngle = data.losAngle;
        range = data.range;
        fireRate = data.fireRate;
        timer = 1 / fireRate;
        activeAugments = new List<Augments>(AugmentManager.Instance.activeAugments);
    }

    public void SetData(SentryData data)
    {
        this.data = data;
        if (data)
            SetValue();
    }

    public void AddNewAugment(Augments augmentToAdd)
    {
        
    }

    public void AddActiveAugment(GameObject bullet, Projectile proj)
    {
        foreach (Augments augmentToAdd in AugmentManager.Instance.activeAugments)
        {
            AugmentManager.Instance.AddAugmentToProjectile(augmentToAdd, bullet, proj);
            proj.activeAugments.Add(augmentToAdd);
        }
    }

    public void AddAugmentToList(Augments augmentToAdd)
    {
        if (!activeAugments.Contains(augmentToAdd) && !augmentToAdd.Equals(Augments.None))
        {
            activeAugments.Add(augmentToAdd);
        }
    }
}
