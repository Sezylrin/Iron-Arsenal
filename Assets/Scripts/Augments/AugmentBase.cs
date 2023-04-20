using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBase : MonoBehaviour
{
    // Start is called before the first frame update
    public Projectile baseProjectile;

    public Collider lastHit;

    public float baseValue;

    public bool respawned;

    public StatAttribute AugmentAttribute = StatAttribute.Undefined;

    public void Respawn()
    {
        respawned = true;
        switch ((int)AugmentAttribute)
        {
            case (int)StatAttribute.Physical:
                baseValue = StatsManager.Instance.physicalDamage;
                break;
            case (int)StatAttribute.Elemental:
                baseValue = StatsManager.Instance.elementalDamage;
                break;
            case (int)StatAttribute.Health:
                baseValue = StatsManager.Instance.healthFactor;
                break;
        }
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
