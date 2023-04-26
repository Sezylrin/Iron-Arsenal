using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBase : MonoBehaviour
{
    // Start is called before the first frame update
    public Projectile baseProjectile;

    public Collider lastHit;

    public bool respawned;

    public StatAttribute AugmentAttribute = StatAttribute.Undefined;

    public void Respawn()
    {
        respawned = true;
        
        lastHit = null;
    }

    public virtual void Init()
    {

    }
    public void Initi()
    {
        this.enabled = true;
    }

    public void DisableAugment()
    {
        this.enabled = false;
    }




}
