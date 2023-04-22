using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [field: Header("Wave Management")]
    [field: SerializeField] public int Wave { get; private set; }
    [field: SerializeField] private int WaveDelay { get; set; }
    [field: SerializeField] public int SecondsUntilNextWave { get; private set; }
    [field: SerializeField] private float BasicEnemyChance { get; set; }
    [field: SerializeField] private float SpecialEnemyChance { get; set; }
    [field: SerializeField] public bool IsBossAlive { get; private set; }
    [field: SerializeField] private bool SpawningWave { get; set; }
    public bool debugStartWaveNow; // Testing Variable

    [field: Header("Lists")]
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject[] augmentPrefabs;
    public List<Transform> enemyList = new List<Transform>();

    private GameObject Player { get; set; }
    private Vector3 PlayerPosition { get; set; }
    private BaseFunctions playerFunction;

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

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
        Player = GameObject.Find("Player");
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

        SpecialEnemyChance = (100 - BasicEnemyChance) / (pools.Count - 1);

        IsBossAlive = false;
        SpawningWave = false;

        Wave -= 1;
        SecondsUntilNextWave = 0;

        StartCoroutine(DelayWave());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = Player.transform.position;

        if (debugStartWaveNow)
        {
            debugStartWaveNow = false;
            StartWaveEarly();
        }
    }

    private void SelectWave()
    {
        Wave++;

        if (Wave <= 0)
        {
            Wave = 1;
        }

        if (Wave % 10 == 0)
        {
            StartCoroutine(SpawnBossWave());
        }
        else StartCoroutine(SpawnWave());
    }

    private IEnumerator DelayWave()
    {
        SecondsUntilNextWave = WaveDelay;
        
        while (SecondsUntilNextWave > 0)
        {
            yield return new WaitForSeconds(1);
            SecondsUntilNextWave--;
        }
        
        SecondsUntilNextWave = 0;
        SelectWave();
    }

    public void StartWaveEarly()
    {
        if (!SpawningWave)
        {
            SecondsUntilNextWave = 0;
        }
    }

    private IEnumerator SpawnWave()
    {
        SpawningWave = true;
        int numberToSpawn = 10 + Wave;

        while (numberToSpawn != 0)
        {
            yield return new WaitForSeconds(0.25f);

            SpawnEnemy(false);

            numberToSpawn--;
        }

        SpawningWave = false;
        StartCoroutine(DelayWave());
    }

    private IEnumerator SpawnBossWave()
    {
        SpawnEnemy(true);
        IsBossAlive = true;

        while (IsBossAlive)
        {
            yield return new WaitForSeconds(2f);
            SpawnEnemy(false);
        }
        StartCoroutine(DelayWave());
    }

    public void BossDeath()
    {
        IsBossAlive = false;
    }

    private int SelectEnemyToSpawn(bool isBoss)
    {
        if (isBoss)
        {
            return Random.Range(0, bossPrefabs.Length);
        }
        else
        {
            float random = Random.Range(1f, 100f);

            for (int i = 1; i < pools.Count; i++)
            {
                if ((BasicEnemyChance + (SpecialEnemyChance * (i - 1))) < random && random <= (BasicEnemyChance + (SpecialEnemyChance * i)))
                {
                    return i;
                }
            }
        }
        return 0;
    }

    private void SpawnEnemy(bool isBoss)
    {
        int enemyType;
        GameObject newEnemy = null;

        if (isBoss)
        {
            enemyType = SelectEnemyToSpawn(true);
            newEnemy = Instantiate(bossPrefabs[enemyType], gameObject.transform);
        }
        else
        {
            enemyType = SelectEnemyToSpawn(false);

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
        }

        SetValues(newEnemy);
    }

    private void SetValues(GameObject enemy)
    {
        enemy.transform.position = GetRandomPosition();
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetStats();
        enemyScript.InitEnemyEffects(augmentPrefabs);
        enemyList.Add(enemy.transform);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 Top = new Vector3(PlayerPosition.x, 0, PlayerPosition.z + 30);
        Vector3 Bottom = new Vector3(PlayerPosition.x, 0, PlayerPosition.z - 30);
        Vector3 Right = new Vector3(PlayerPosition.x + 30, 0, PlayerPosition.z);
        Vector3 Left = new Vector3(PlayerPosition.x - 30, 0, PlayerPosition.z);

        float x = 0;
        float z = 0;

        int randomEdge = Random.Range(1, 5);

        if (randomEdge == 1) // Top
        {
            x = Random.Range(Left.x, Right.x);
            z = Random.Range(Top.z, Top.z + 20);
        }
        else if (randomEdge == 2) // Right
        {
            z = Random.Range(Top.z, Bottom.z);
            x = Random.Range(Right.x, Right.x + 20);
        }
        else if (randomEdge == 3) // Bottom
        {
            x = Random.Range(Left.x, Right.x);
            z = Random.Range(Bottom.z, Bottom.z - 20);
        }
        else if (randomEdge == 4) // Left
        {
            z = Random.Range(Top.z, Bottom.z);
            x = Random.Range(Left.x, Left.x - 20);
        }
        return new Vector3(x, 0, z);
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
