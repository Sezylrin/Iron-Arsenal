
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float damageFactor;
    public float bulletSpeed;
    public int pierce = 0;

    public Mesh projectileMesh;
}
