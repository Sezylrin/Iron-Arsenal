using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APoison : AugmentBase
{
    // Start is called before the first frame update
    private bool isPoison;
    void Start()
    {
        
    }
    public override void Init()
    {
        if (baseProjectile.GetOwner())
            isPoison = baseProjectile.GetOwner().data.Sentry.Equals(SentryName.PoisonTower);
        if (baseProjectile.GetCannonOwner())
            isPoison = baseProjectile.GetCannonOwner().type.Equals(CannonProjectileType.PoisonShot);

        AugmentAttribute = isPoison ? StatAttribute.Elemental : StatAttribute.Physical;
        base.Init();
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
            //Debug.Log((int)(5 * (baseProjectile.modifiedDamage / baseValue)));
            EnemyEffects effect = other.gameObject.GetComponent<EnemyEffects>();
            Debug.Log("running");
            if (isPoison)
            {
                effect.poisonTick += 5;
            }
            else if(Random.Range(0,100f) < (baseProjectile.data.damageFactor) * 100)
            {
                effect.poisonTick += 2;
            }
        }
    }
}
