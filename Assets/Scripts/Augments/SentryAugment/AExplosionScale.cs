using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AExplosionScale : AugmentBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (respawned && baseProjectile.activeAugments.Contains(Augments.ExplosiveProjectile))
        {
            respawned = false;
            GetComponent<AExplosive>().SetScale(2);
            //this.enabled = false;
        }
    }
}
