using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowShot : CannonProjectile
{
    private float slowStrength;

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
        slowStrength = 0.5f * Mathf.Pow(1.02f, 1 - data.level);
    }

    public override void SetStats(Cannon owner, Vector3 direction, float damage, float projectileSpeed, float fireDelay)
    {
        base.SetStats(owner, direction, damage, projectileSpeed, fireDelay);
        slowStrength = 0.5f * Mathf.Pow(1.02f, 1 - data.level);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            other.gameObject.GetComponent<Enemy>().StartSlow(slowStrength);
            DeleteNow();
        }

        if (other.gameObject.tag == "Wall")
        {
            DeleteNow();
        }
    }
}
