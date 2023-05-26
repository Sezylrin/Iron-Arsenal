using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{
    public GameObject basicEnemy;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    protected override void OnDeath()
    {
        Manager.SpawnEnemy(false, 0, spawnPoint1.position);
        Manager.SpawnEnemy(false, 0, spawnPoint2.position);
        base.OnDeath();
    }
}
