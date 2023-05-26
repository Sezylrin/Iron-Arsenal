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

    public GameObject projectilePF;

    private float timer;

    public Transform bulletSpawnpoint;

    private Pooling pooledBullets = new Pooling();

    public Transform sentryHead;

    public SentryData data;

    private GameObject poolObject;

    public List<Augments> activeAugments;

    private StatAttribute attributeType;

    private SentryEffects sentryEffects;

    public ParticleSystem muzzleFlash;

    public bool isDouble = false;
    void Start()
    {
        
        
    }

    public void init()
    {
        if (!centerPoint)
            centerPoint = GameObject.FindWithTag("CenterPoint").transform;
        if (!centerPoint)
        {
            Debug.Log("No centerPoint for turrets to reference, please create an emepty transform at the center of the base");
            Debug.Break();
        }
        if (!sentryEffects)
        {
            sentryEffects = gameObject.AddComponent<SentryEffects>();
            sentryEffects.hostSentry = this;

        }
        if (AugmentManager.Instance)
        {
            activeAugments = new List<Augments>(AugmentManager.Instance.activeAugments);
            UpdateAugments();
            AugmentManager.Instance.activeSentries.Add(this);
        }
        poolObject = new GameObject("Projectile Storage");


    }

    // Update is called once per frame
    void Update()
    {
        if (!data)
            return;
        SetForward();
        if (!target)
        {
            LocateTarget(EnemyManager.Instance.enemyList);
        }
        else
        {
            TargetCheck(target);
            if (target)
            {
                if (timer <= 0)
                {
                    if (!isDouble)
                        ShootTarget(Vector3.zero);
                    else
                    {
                        ShootTarget(Vector3.right * 0.25f);
                        ShootTarget(Vector3.left * 0.25f);
                    }
                    timer = 1 / fireRate;
                }

            }
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

    private void ShootTarget(Vector3 offSet)
    {
        muzzleFlash.Play();
        
        GameObject bullet;
        Projectile bulletProj;
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
            bullet = Instantiate(projectilePF, Vector3.zero, Quaternion.identity,poolObject.transform);

        }
        bulletProj = bullet.GetComponent<Projectile>();
        bulletProj.SetProjectileData(data.projectileData, this, attributeType);
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
        else if (activeAugments.Count - bulletProj.activeAugments.Count > 1 )
        {
            AddAllAugments(bullet, bulletProj);
        }
        foreach (AugmentBase augmentBase in bulletProj.augmentBases)
        {
            augmentBase.Respawn();
        }
        bulletProj.setSpawn(bulletSpawnpoint.position);
        Vector3 targetDir = target.position - bulletSpawnpoint.position;
        targetDir.y = 0;
        bulletProj.SetRotation(targetDir, offSet);
        bulletProj.dir = targetDir.normalized;
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
        attributeType = data.Attribute;
        losAngle = data.losAngle;
        range = data.range;
        fireRate = data.fireRate;
        timer = 1 / fireRate;
        init();
        if (data.defaultAugment.Length > 0)
        {
            foreach (Augments augment in data.defaultAugment)
            {
                AddAugmentToList(augment);
            }
        }
        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            rend.material = data.mat;
        }
    }

    public void SetData(SentryData data)
    {
        this.data = data;
        if (data)
            SetValue();
    }

    public void AddAugmentToList(Augments augmentToAdd)
    {
        if (!activeAugments.Contains(augmentToAdd) && !augmentToAdd.Equals(Augments.None))
        {
            activeAugments.Add(augmentToAdd);
        }
        UpdateAugments();
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

    public void UpdateAugments()
    {
        if (!sentryEffects)
        {
            sentryEffects = gameObject.AddComponent<SentryEffects>();
            sentryEffects.hostSentry = this;

        }
        sentryEffects.UpdateAugments();
    }

    public void DestroyPool()
    {
        Destroy(poolObject);
    }

    public void RefundCost()
    {
        LevelManager.Instance.GainNovacite(Mathf.FloorToInt(data.novaciteCost * 0.7f));
        LevelManager.Instance.GainVoidStone(Mathf.FloorToInt(data.voidStoneCost * 0.7f));
        LevelManager.Instance.GainXenorium(Mathf.FloorToInt(data.xenoriumCost * 0.7f));
    }
}
