using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICannonProjectile
{
    Vector3 Direction { get; set; }
    float Damage { get; set; }
    float ProjectileSpeed { get; set; }
    float FireDelay { get; set; }

}
