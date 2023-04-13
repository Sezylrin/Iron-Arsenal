using UnityEngine;

[CreateAssetMenu(fileName = "CannonProjectileData", menuName = "ScriptableObjects/CannonProjectileData")]
public class CannonProjectileData : ScriptableObject
{
    public int level;
    public float damage;
    public float projectileSpeed;
    public float fireDelay;
    public float projectileLifetime;
}
