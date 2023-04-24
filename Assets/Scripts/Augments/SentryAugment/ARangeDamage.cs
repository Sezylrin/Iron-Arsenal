using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARangeDamage : AugmentBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rate = baseProjectile.distanceTravelled / baseProjectile.maxDistance;
        if (rate > 0.5f)
        {
            baseProjectile.damageScale = 0.5f + rate;
        }
    }
}
