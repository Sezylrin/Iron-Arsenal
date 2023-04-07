using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject player;
    public GameObject[] enemyPrefabs;


    
    public List<Transform> enemyList = new List<Transform>();


    private Pooling pooledBasicEnemies = new Pooling();     //0
    private Pooling pooledTankEnemies = new Pooling();      //1
    private Pooling pooledExploderEnemies = new Pooling();  //2
    private Pooling pooledDiggerEnemies = new Pooling();    //3
    private Pooling pooledChargerEnemies = new Pooling();   //4
    private Pooling pooledSentryEnemies = new Pooling();    //5
    private Pooling pooledEnemyBullets = new Pooling();     //6
    public List<Pooling> pools = new List<Pooling>();

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, 5);

        pools.Add(pooledBasicEnemies); 
        pools.Add(pooledTankEnemies); 
        pools.Add(pooledExploderEnemies);
        pools.Add(pooledDiggerEnemies);
        pools.Add(pooledChargerEnemies);
        pools.Add(pooledSentryEnemies);
        pools.Add(pooledEnemyBullets);
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
        GameObject temp2 = Instantiate(enemyPrefabs[0], temp, Quaternion.identity);
        enemyList.Add(temp2.transform);
    }

    public void PoolBasicEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledBasicEnemies.AddObj(obj);
    }

    public void PoolTankEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledTankEnemies.AddObj(obj);
    }

    public void PoolExploderEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledExploderEnemies.AddObj(obj);
    }

    public void PoolDiggerEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledDiggerEnemies.AddObj(obj);
    }

    public void PoolChargerEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledChargerEnemies.AddObj(obj);
    }

    public void PoolSentryEnemy(GameObject obj)
    {
        obj.SetActive(false);
        pooledSentryEnemies.AddObj(obj);
    }

    public void PoolEnemyBullet(GameObject obj)
    {
        obj.SetActive(false);
        pooledEnemyBullets.AddObj(obj);
    }
}
