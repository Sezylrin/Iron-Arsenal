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

    private GameObject newEnemy = null;

    public int wave;
    public int waveDelay;

    public bool spawnInstantly; // Testing Variable

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
        pools.Add(pooledDodgerEnemies);
        pools.Add(pooledSplitterEnemies);
        pools.Add(pooledCloakerEnemies);

        specialEnemyChance = (100 - basicEnemyChance) / (pools.Count - 1);

        StartCoroutine(DelayWave(waveDelay));
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
    }

    IEnumerator DelayWave(int delay)
    {
        if (wave <= 0)
        {
            wave = 1;
        }

        while (true)
        {
            if (!spawnInstantly)
            {
                yield return new WaitForSeconds(delay);
            }

            StartCoroutine(SpawnEnemies(10 + wave));

            if (spawnInstantly)
            {
                yield return new WaitForSeconds(delay);
            }
            wave++;
        }
    }

    public IEnumerator SpawnEnemies(int numberOfSpawns)
    {
        int numberToSpawn = numberOfSpawns;

        while (numberToSpawn != 0)
        {
            yield return new WaitForSeconds(0.3f);

            Vector3 Top = new Vector3(playerPosition.x, 0, playerPosition.z + 30);
            Vector3 Bottom = new Vector3(playerPosition.x, 0, playerPosition.z - 30);
            Vector3 Right = new Vector3(playerPosition.x + 30, 0, playerPosition.z);
            Vector3 Left = new Vector3(playerPosition.x - 30, 0, playerPosition.z);

            float x = 0;
            float z = 0;

            if (numberToSpawn % 4 == 0) // Top
            {
                x = Random.Range(Left.x, Right.x);
                z = Random.Range(Top.z, Top.z + 20);
            }
            else if (numberToSpawn % 4 == 1) // Right
            {
                z = Random.Range(Top.z, Bottom.z);
                x = Random.Range(Right.x, Right.x + 20);
            }
            else if (numberToSpawn % 4 == 2) // Bottom
            {
                x = Random.Range(Left.x, Right.x);
                z = Random.Range(Bottom.z, Bottom.z - 20);
            }
            else if (numberToSpawn % 4 == 3) // Left
            {
                z = Random.Range(Top.z, Bottom.z);
                x = Random.Range(Left.x, Left.x - 20);
            }
            Vector3 spawnPosition = new Vector3(x, 0, z);
                
            float random = Random.Range(1f, 100f);

            SpawnEnemy(SelectEnemyToSpawn(random), spawnPosition);
            
            numberToSpawn--;
        }
        yield return null;
    }

    public int SelectEnemyToSpawn(float randomValue)
    {
        for (int i = 1; i < pools.Count; i++)
        {
            if ((basicEnemyChance + (specialEnemyChance * (i - 1))) < randomValue && randomValue <= (basicEnemyChance + (specialEnemyChance * i)))
            {
                return i;
            }
        }
        return 0;
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
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetStats();

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
