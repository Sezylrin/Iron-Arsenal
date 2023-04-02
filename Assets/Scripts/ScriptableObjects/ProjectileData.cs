
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float baseDamage;
    public float bulletSpeed;
    public int pierce = 0;
    public bool isExplosive;

    public Mesh projectileMesh;
}
