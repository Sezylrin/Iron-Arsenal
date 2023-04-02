
using UnityEngine;

[CreateAssetMenu(fileName = "SentryData",menuName ="ScriptableObjects/SentryData")]
public class SentryData : ScriptableObject
{
    public Mesh SentryBase;
    public Mesh SentryTurret;

    public float range;
    public float losAngle;
    public float fireRate;

    public ProjectileData projectileData;
}
