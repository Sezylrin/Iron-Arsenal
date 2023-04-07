using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPosition;
    public GameObject[] enemyPrefabs;


    
    public List<Transform> enemyList = new List<Transform>();


    private Pooling pooledBasicEnemies = new Pooling();     //0 - Basic Enemies
    private Pooling pooledTankEnemies = new Pooling();      //1 - Tank Enemies
    private Pooling pooledExploderEnemies = new Pooling();  //2 - Exploder Enemies
    private Pooling pooledDiggerEnemies = new Pooling();    //3 - Digger Enemies
    private Pooling pooledChargerEnemies = new Pooling();   //4 - Charger Enemies
    private Pooling pooledSentryEnemies = new Pooling();    //5 - Sentry Enemies
    public List<Pooling> pools = new List<Pooling>();

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        pools.Add(pooledBasicEnemies); 
        pools.Add(pooledTankEnemies); 
        pools.Add(pooledExploderEnemies);
        pools.Add(pooledDiggerEnemies);
        pools.Add(pooledChargerEnemies);
        pools.Add(pooledSentryEnemies);

        StartCoroutine(SpawnEnemies(100));
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
    }

    public IEnumerator SpawnEnemies(int numberOfSpawns)
    {
        while (true)
        {
            //yield return new WaitForSeconds(30);
            int numberToSpawn = numberOfSpawns;
            while (numberToSpawn != 0)
            {
                Vector3 Top = new Vector3(playerPosition.x, 0, playerPosition.z + 30);
                Vector3 Bottom = new Vector3(playerPosition.x, 0, playerPosition.z - 30);
                Vector3 Right = new Vector3(playerPosition.x + 30, 0, playerPosition.z);
                Vector3 Left = new Vector3(playerPosition.x - 30, 0, playerPosition.z);

                float x = 0;
                float z = 0;

                if (numberToSpawn % 4 == 0) // Top
                {
                    x = Random.Range(Left.x, Right.x);
                    z = Random.Range(Top.z, Top.z + 10);
                }
                else if (numberToSpawn % 4 == 1) // Right
                {
                    z = Random.Range(Top.z, Bottom.z);
                    x = Random.Range(Right.x, Right.x + 10);
                }
                else if (numberToSpawn % 4 == 2) // Bottom
                {
                    x = Random.Range(Left.x, Right.x);
                    z = Random.Range(Bottom.z, Bottom.z - 10);
                }
                else if (numberToSpawn % 4 == 3) // Left
                {
                    z = Random.Range(Top.z, Bottom.z);
                    x = Random.Range(Left.x, Left.x - 10);
                }

                Vector3 spawnPosition = new Vector3(x, 0.3f, z);

                int random = Random.Range(1, 101);
                GameObject newEnemy = null;

                if (random <= 50) //Spawn Basic Enemy (50% Chance)
                {
                    if (pooledBasicEnemies.ListCount() > 0)
                    {
                        newEnemy = pooledBasicEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledBasicEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[0], gameObject.transform);
                    }
                }
                else if (50 < random && random <= 60) //Spawn Tank Enemy (10% Chance)
                {
                    if (pooledTankEnemies.ListCount() > 0)
                    {
                        newEnemy = pooledTankEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledTankEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[1], gameObject.transform);
                    }
                }
                else if (60 < random && random <= 70) //Spawn Exploder Enemy (10% Chance)
                {
                    if (pooledExploderEnemies.ListCount() > 0)
                    {
                        newEnemy = pooledExploderEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledExploderEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[2], gameObject.transform);
                    }
                }
                else if (70 < random && random <= 80) //Spawn Digger Enemy (10% Chance)
                {
                    if (pooledDiggerEnemies.ListCount() > 0)
                    {
                        newEnemy = pooledDiggerEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledDiggerEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[3], gameObject.transform);
                    }
                }
                else if (80 < random && random <= 90)
                {
                    if (pooledChargerEnemies.ListCount() > 0)  //Spawn Charger Enemy (10% Chance)
                    {
                        newEnemy = pooledChargerEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledChargerEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[4], gameObject.transform);
                    }
                }
                else if (90 < random && random <= 100) //Spawn Sentry Enemy (10% Chance)
                {
                    if (pooledSentryEnemies.ListCount() > 0)
                    {
                        newEnemy = pooledSentryEnemies.FirstObj();
                        newEnemy.SetActive(true);
                        pooledSentryEnemies.RemoveObj(newEnemy);
                    }
                    else
                    {
                        newEnemy = Instantiate(enemyPrefabs[5], gameObject.transform);
                    }
                }
                newEnemy.transform.position = spawnPosition;
                
                enemyList.Add(newEnemy.transform);

                numberToSpawn--;
            }
            yield return new WaitForSeconds(30);
        }
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
}
