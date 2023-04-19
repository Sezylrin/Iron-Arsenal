using UnityEngine;


[CreateAssetMenu(fileName = "EnemyBulletData",menuName = "ScriptableObjects/EnemyBulletData")]
public class EnemyBulletData : ScriptableObject
{
    public float damage;
    public float projectileSpeed;
    public float fireDelay;
    public float projectileLifetime;
}
