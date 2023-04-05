using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICannonProjectile
{
    Cannon Owner { get; set; }
    Vector3 Direction { get; set; }
    float Damage { get; set; }
    float ProjectileSpeed { get; set; }
    float FireDelay { get; set; }
    void Shoot();
    void Delete();
}
