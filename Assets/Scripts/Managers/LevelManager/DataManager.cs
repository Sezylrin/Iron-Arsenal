using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public List<CannonProjectileData> cannonProjectileData = new List<CannonProjectileData>();
    public List<EnemyData> enemyData = new List<EnemyData>();

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        SetCannonProjectilesToLevel1();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCannonProjectilesToLevel1()
    {
        for (int i = 0; cannonProjectileData.Count > i; i++)
        {
            cannonProjectileData[i].level = 1;
        }
    }

    public void UpgradeCannonProjectile(CannonProjectileType type)
    {
        switch (type)
        {
            case CannonProjectileType.Bullet:
                cannonProjectileData[0].level += 1;
                break;
            case CannonProjectileType.Shotgun:
                cannonProjectileData[1].level += 1;
                break;
            case CannonProjectileType.RapidFire:
                cannonProjectileData[2].level += 1;
                break;
            case CannonProjectileType.SlowShot:
                cannonProjectileData[3].level += 1;
                break;
            case CannonProjectileType.PoisonShot:
                cannonProjectileData[4].level += 1;
                break;
            case CannonProjectileType.Rocket:
                cannonProjectileData[5].level += 1;
                break;
            case CannonProjectileType.Flame:
                cannonProjectileData[6].level += 1;
                break;
        }
    }
}
