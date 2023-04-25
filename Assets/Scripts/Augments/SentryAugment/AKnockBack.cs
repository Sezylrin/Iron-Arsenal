using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKnockBack : AugmentBase
{
    // Start is called before the first frame update


    private float force = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (respawned)
        {
            lastHit = null;
            respawned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && lastHit != other)
        {
            lastHit = other;            
            other.GetComponent<Rigidbody>().AddForce(baseProjectile.dir * force * baseProjectile.data.damageFactor, ForceMode.Impulse);
        }
    }
}
