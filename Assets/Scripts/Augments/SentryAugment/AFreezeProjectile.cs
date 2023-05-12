using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFreezeProjectile : AugmentBase
{
    // Start is called before the first frame update
    private bool isFreeze = false;
    void Start()
    {
        if (baseProjectile.GetOwner())
        {
            isFreeze = baseProjectile.GetOwner().data.Sentry.Equals(SentryName.FreezeTower);
            isFreeze = baseProjectile.GetOwner().data.Sentry.Equals(SentryName.IceShardTower);
        }
        if (baseProjectile.GetCannonOwner())
            isFreeze = baseProjectile.GetCannonOwner().type.Equals(CannonProjectileType.SlowShot);

        AugmentAttribute = isFreeze ? StatAttribute.Elemental : StatAttribute.Physical;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && lastHit != other)
        {
            lastHit = other;
            other.gameObject.GetComponent<EnemyEffects>().IncreaseFreeze(isFreeze ? 10 : (int)(5 * baseProjectile.data.damageFactor));
        }
    }
}
