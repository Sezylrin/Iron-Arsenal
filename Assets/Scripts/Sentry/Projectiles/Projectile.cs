using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public ProjectileData data;
    
    public Vector3 dir;

    public float modifiedDamage;

    public float modifiedSpeed;

    public int modifiedPierce;

    private Sentry owner;

    private CannonProjectile cannonOwner;

    public List<Augments> activeAugments = new List<Augments>();

    public List<AugmentBase> augmentBases = new List<AugmentBase>();

    private Collider lastHit = null;

    public float maxDistance;

    private Vector3 spawnPos;

    public float distanceTravelled;

    public float damageScale = 1;

    public MeshFilter projMesh;

    public BoxCollider boxCollider;

    private StatAttribute attribute;

    private bool isDouble = false;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!data)
            return;
        rb.velocity = dir * modifiedSpeed;
        distanceTravelled = Vector3.Distance(transform.position, spawnPos);
        if (distanceTravelled >= maxDistance)
        {
            if (owner)
                owner.PoolBullet(gameObject);
            if (cannonOwner)
                cannonOwner.PoolProj(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void SetProjectileStat()
    {
        float damage = attribute.Equals(StatAttribute.Physical) ? StatsManager.Instance.physicalDamage : StatsManager.Instance.elementalDamage;
        modifiedDamage = data.damageFactor * damage;
        if (!isDouble)
            activeAugments.Contains(Augments.DoubleProjectiles);
        else
        {
            modifiedDamage *= 0.65f;
        }
        modifiedSpeed = data.bulletSpeed;
        modifiedPierce = data.pierce;
        maxDistance = data.maxDistance;
        if (!data.colliderSize.Equals(Vector3.zero))
            boxCollider.size = data.colliderSize;
        if (data.invisible)
            projMesh.mesh = null;
        damageScale = 1;
    }

    public void SetRotation(Vector3 dir, Vector3 offset)
    {
        transform.LookAt(transform.position + dir, Vector3.up);
        transform.Translate(offset);
    }

    public void setSpawn(Vector3 pos)
    {
        transform.position = pos;
        spawnPos = pos;
    }
    public void SetProjectileData(ProjectileData data, Sentry owner,StatAttribute attribute)
    {
        if (!this.data)
            this.data = data;
        if (!this.owner)
            this.owner = owner;
        this.attribute = attribute;
        if (data.mat)
        {
            GetComponentInChildren<MeshRenderer>().material = data.mat;
        }
        SetProjectileStat();
    }
    public void SetProjectileData(ProjectileData data, CannonProjectile owner, StatAttribute attribute)
    {
        if (!this.data)
            this.data = data;
        if (!this.cannonOwner)
            cannonOwner = owner;
        this.attribute = attribute;
        if (data.mat)
        {
            GetComponentInChildren<MeshRenderer>().material = data.mat;
        }
        SetProjectileStat();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && lastHit != other)
        {
            lastHit = other;
            other.gameObject.GetComponent<Enemy>().TakeDamage(modifiedDamage * damageScale);
            if (modifiedPierce <= 0)
            {
                lastHit = null;
                if (owner)
                    owner.PoolBullet(gameObject);
                if (cannonOwner)
                    cannonOwner.PoolProj(gameObject);
                gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.tag == "Wall")
        {
            if (owner)
                owner.PoolBullet(gameObject);
            if (cannonOwner)
                cannonOwner.PoolProj(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            modifiedPierce--;
    }

    public Sentry GetOwner()
    {
        return owner;
    }

    public CannonProjectile GetCannonOwner()
    {
        return cannonOwner;
    }
}
