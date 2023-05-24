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

    private LayerMask defaultMask;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject artificalHitbox;
    [SerializeField]
    private GameObject artificalRotate;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
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
        artificalRotate.transform.LookAt(Vector3.forward + artificalRotate.transform.position, Vector3.up);
        artificalHitbox.transform.LookAt(dir + artificalHitbox.transform.position, Vector3.up);
        if (this.dir.z > 0)
            defaultMask = LayerMask.NameToLayer("projectileOver");
        else
            defaultMask = LayerMask.NameToLayer("projectileUnder");
        Invoke("SetLayer", 0.2f);
    }

    private void SetLayer()
    {
        SetLayerRecursively(gameObject, defaultMask);
    }
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public void setSpawn(Vector3 pos, bool IsCannon = false)
    {
        transform.position = pos;
        spawnPos = pos;
        if (artificalHitbox.transform.position.y > 1)
        {
            float offSetAmount = artificalHitbox.transform.position.y - 1;
            Vector3 offSet = new Vector3(0, -offSetAmount, 0);
            if (IsCannon)
                offSet.z = offSetAmount / 26 * 15;
            artificalHitbox.transform.position = offSet + artificalHitbox.transform.position;
        }      
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
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("projectileUnder"));
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
        {
            modifiedPierce--;
            SetLayerRecursively(gameObject, defaultMask);
        }
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
