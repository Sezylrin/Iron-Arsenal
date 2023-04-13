using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBase : MonoBehaviour
{
    // Start is called before the first frame update
    public Projectile baseProjectile;

    public void Init()
    {
        this.enabled = true;
    }

    public void DisableAugment()
    {
        this.enabled = false;
    }


}
