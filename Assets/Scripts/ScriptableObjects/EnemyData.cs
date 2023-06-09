using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float HealthScale;
    public float damageOnCollide;
    public float speed;
    public int ramLaunchMultiplier;
}
