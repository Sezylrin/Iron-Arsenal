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

    private Transform bulletSpawnpoint;

    private Pooling activeBullets = new Pooling();

    private Pooling pooledBullets = new Pooling();

    public SentryData data;
    void Start()
    {
        tempList.Add(testTransform);
        if (!centerPoint)
            centerPoint = GameObject.FindWithTag("CenterPoint").transform;
        if (!centerPoint)
        {
            Debug.Log("No centerPoint for turrets to reference, please create an emepty transform at the center of the base");
            Debug.Break();
        }
        if(!data)
        {
            Debug.Log("Missing Data, taking default values");
            return;
        }
        bulletSpawnpoint = GameObject.FindWithTag("BulletSpawnPoint").transform;
        losAngle = data.losAngle;
        range = data.range;
        fireRate = data.fireRate;
        timer = 1/fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        SetForward();
        if (!target)
            LocateTarget(tempList);
        else
        {
            if (timer <= 0)
            {
                ShootTarget();
                timer = 1/fireRate;
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
            activeBullets.AddObj(bullet);
        }
        else
        {
            //Debug.Log("Spawned new");
            bullet = Instantiate(projectilePF, Vector3.zero, Quaternion.identity);
            activeBullets.AddObj(bullet);
        }
        Projectile bulletProj = bullet.GetComponent<Projectile>();
        bulletProj.SetProjectileData(data.projectileData, this);
        bulletProj.setSpawn(bulletSpawnpoint.position);
        Vector3 targetDir = target.position - transform.position;
        targetDir.y = 0;
        bulletProj.SetDirection(targetDir);
    }


    public void PoolBullet(GameObject obj)
    {
        //Debug.Log("Sent to pool");
        obj.SetActive(false);
        activeBullets.RemoveObj(obj);
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
}
