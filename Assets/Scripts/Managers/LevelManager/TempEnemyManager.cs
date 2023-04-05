using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyManager : MonoBehaviour
{
    public GameObject enemies;
    public Transform player;
    public List<Transform> enemyList = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        Vector3 temp = player.position;
        int x = Random.Range(5, 10);
        int z = Random.Range(5, 10);
        x *= Random.Range(0, 2) == 1 ? 1 : -1;
        z *= Random.Range(0, 2) == 1 ? 1 : -1;
        temp.x += x;
        temp.z += z;
        GameObject temp2 = Instantiate(enemies, temp, Quaternion.identity);
        enemyList.Add(temp2.transform);
    }
}
