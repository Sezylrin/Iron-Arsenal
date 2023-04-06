using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemies;
    private GameObject player;
    public List<Transform> enemyList = new List<Transform>();

    private Pooling pooledEnemyBullets = new Pooling();
    private Pooling pooledBasicEnemies = new Pooling();
  

    public List<Pooling> pools = new List<Pooling>();

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, 5);

        pools.Add(pooledEnemyBullets);
        pools.Add(pooledBasicEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        Vector3 temp = player.transform.position;
        int x = Random.Range(30, 40);
        int z = Random.Range(30, 40);
        x *= Random.Range(0, 2) == 1 ? 1 : -1;
        z *= Random.Range(0, 2) == 1 ? 1 : -1;
        temp.x += x;
        temp.y = 0.3f;
        temp.z += z;
        GameObject temp2 = Instantiate(enemies, temp, Quaternion.identity);
        enemyList.Add(temp2.transform);
    }

    public void PoolEnemyBullet(GameObject obj)
    {
        obj.SetActive(false);
        pooledEnemyBullets.AddObj(obj);
    }

    public void PoolBasicEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledBasicEnemies.AddObj(obj);
    }
}
