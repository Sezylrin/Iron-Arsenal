using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public ProjectileData data;
    
    public Vector3 dir;

    public float baseDamage;

    public float speed;

    public int pierce;

    private Sentry owner;

    public List<Augments> activeAugments = new List<Augments>();

    public List<AugmentBase> augmentBases = new List<AugmentBase>();

    private Collider lastHit = null;

    public float maxDistance = 15;

    private Vector3 spawnPos;

    public float distanceTravelled;

    public float damageScale = 1;

    private StatAttribute attribute;

    private bool isDouble = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!data)
            return;
        transform.Translate(Vector3.forward * modifiedSpeed * Time.deltaTime);
        distanceTravelled = Vector3.Distance(transform.position, spawnPos);
        if (distanceTravelled >= maxDistance)
        {
            owner.PoolBullet(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void TranslateDir()
    {
        transform.Translate(dir * speed * Time.deltaTime);
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
                owner.PoolBullet(gameObject);
                gameObject.SetActive(false);
            }
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
}
