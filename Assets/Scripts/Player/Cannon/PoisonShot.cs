using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShot : CannonProjectile
{
    public float damageOverTime;

    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void SetStats()
    {
        base.SetStats();
        damageOverTime = Damage / 2;
    }

    public override void SetStats(Cannon owner, Vector3 direction, float damage, float projectileSpeed, float fireDelay)
    {
        base.SetStats(owner, direction, damage, projectileSpeed, fireDelay);
        damageOverTime = Damage / 2;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage); 
            other.gameObject.GetComponent<Enemy>().StartDamageOverTime(damageOverTime);
            DeleteNow();
        }

        if (other.gameObject.tag == "Wall")
        {
            DeleteNow();
        }
    } 
}
