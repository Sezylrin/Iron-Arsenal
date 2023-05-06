
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float damageFactor;
    public float bulletSpeed;
    public int pierce = 0;
    public float maxDistance;
    public Vector3 colliderSize = Vector3.zero;
    public bool invisible = false;

    public Mesh projectileMesh;

    public Material mat;
}
