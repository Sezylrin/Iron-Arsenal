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
    private Pooling pooledDodgerEnemies = new Pooling();    //6 - Dodger Enemies
    private Pooling pooledSplitterEnemies = new Pooling();  //7 - Splitter Enemies
    private Pooling pooledCloakerEnemies = new Pooling();   //8 - Cloaker Enemies
    public List<Pooling> pools = new List<Pooling>();

    public Pooling pooledEnemyBullets = new Pooling();

    public float basicEnemyChance;
    private float specialEnemyChance;

    public bool spawnInstantly;

    private GameObject newEnemy = null;

    public int wave;

    private void Awake()
    {
        player = GameObject.Find("Player");
        wave = 1;
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
        pools.Add(pooledDodgerEnemies);
        pools.Add(pooledSplitterEnemies);
        pools.Add(pooledCloakerEnemies);

        basicEnemyChance = 50;
        specialEnemyChance = (100 - basicEnemyChance) / (pools.Count - 1);

        StartCoroutine(SpawnEnemies(24 + wave));
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
    }

    public IEnumerator SpawnEnemies(int numberOfSpawns)
    {
        if (!spawnInstantly)
        {
            yield return new WaitForSeconds(30);
        }

        while (true)
        {
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

                Vector3 spawnPosition = new Vector3(x, 1, z);

                
                float random = Random.Range(1f, 100f);

                if (random <= basicEnemyChance) //Spawn Basic Enemy (0)
                {
                    SpawnEnemy(0, spawnPosition);
                }
                else if (CheckIfSpawn(random, 1)) //Spawn Tank Enemy (1)
                {
                    SpawnEnemy(1, spawnPosition);
                }
                else if (CheckIfSpawn(random, 2)) //Spawn Exploder Enemy (2)
                {
                    SpawnEnemy(2, spawnPosition);
                }
                else if (CheckIfSpawn(random, 3)) //Spawn Digger Enemy (3)
                {
                    SpawnEnemy(3, spawnPosition);
                } 
                else if (CheckIfSpawn(random, 4)) //Spawn Charger Enemy (4)
                {
                    SpawnEnemy(4, spawnPosition);
                }
                else if (CheckIfSpawn(random, 5)) //Spawn Sentry Enemy (5)
                {
                    SpawnEnemy(5, spawnPosition);
                }
                else if (CheckIfSpawn(random, 6)) //Spawn Dodger Enemy (6)
                {   
                    SpawnEnemy(6, spawnPosition);
                }
                else if (CheckIfSpawn(random, 7)) //Spawn Splitter Enemy (7)
                {
                    SpawnEnemy(7, spawnPosition);
                }
                else if (CheckIfSpawn(random, 8)) //Spawn Cloaker Enemy (8)
                {
                    SpawnEnemy(8, spawnPosition);
                }
                numberToSpawn--;
            }
            if (spawnInstantly)
            {
                yield return new WaitForSeconds(30);
            }
        }
    }

    public bool CheckIfSpawn(float randomValue, int enemyNumber)
    {
        return (basicEnemyChance + (specialEnemyChance * (enemyNumber - 1))) < randomValue && randomValue <= (basicEnemyChance + (specialEnemyChance * enemyNumber));
    }

    public void SpawnEnemy(int enemyType, Vector3 spawnPosition)
    {
        if (pools[enemyType].ListCount() > 0)
        {
            newEnemy = pools[enemyType].FirstObj();
            newEnemy.SetActive(true);
            pools[enemyType].RemoveObj(newEnemy);
        }
        else
        {
            newEnemy = Instantiate(enemyPrefabs[enemyType], gameObject.transform);
        }

        newEnemy.transform.position = spawnPosition;
        IEnemy enemyScript = newEnemy.GetComponent<IEnemy>();
        enemyScript.CurrentHealth = enemyScript.MaxHealth;
        enemyScript.SetStats(wave);

        enemyList.Add(newEnemy.transform);
    }

    public void PoolEnemy(GameObject obj, int enemyType)
    {
        obj.transform.Translate(Vector3.down * -1000);
        obj.SetActive(false);
        pools[enemyType].AddObj(obj);
    }

    public void PoolEnemyBullet(GameObject obj)
    {
        obj.SetActive(false);
        pooledEnemyBullets.AddObj(obj);
    }
}
