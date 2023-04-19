using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFireProjectiles : AugmentBase
{
    // Start is called before the first frame update
    private bool isFlame = false;
    public override void Init()
    {
        if (baseProjectile.GetOwner().data.Sentry.Equals(SentryName.FlameThrower))
        {
            isFlame = true;
        }

        AugmentAttribute = isFlame? StatAttribute.Elemental : StatAttribute.Physical;
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
            other.gameObject.GetComponent<EnemyEffects>().fireTick += isFlame? 5 : (int)(5 * (baseProjectile.modifiedDamage/baseValue));
        }
    }
}
