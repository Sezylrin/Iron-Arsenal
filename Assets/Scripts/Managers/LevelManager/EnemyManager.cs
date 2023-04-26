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
    [field: SerializeField] private bool WaveActive { get; set; }
    [field: SerializeField] private bool IsBossWave { get; set; }
    [field: SerializeField] public int SecondsUntilNextWave { get; private set; }
    [field: SerializeField] private float BasicEnemyChance { get; set; }
    [field: SerializeField] private float SpecialEnemyChance { get; set; }
    [field: SerializeField] public int SafeSpawnArea { get; private set; }
    [field: SerializeField] public int BossWaveFrequency { get; private set; }
    [field: SerializeField] public bool IsBossAlive { get; private set; }
    [field: SerializeField] private int PreviousBoss { get; set; }
    [field: SerializeField] private bool SpawningWave { get; set; }
    public bool debugStartWaveNow; // Testing Variable

    [field: Header("Lists")]
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject[] augmentPrefabs;
    public List<Transform> enemyList = new List<Transform>();
    public List<GameObject> WaveList = new List<GameObject>();

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
    private Pooling pooledBursterEnemies = new Pooling();   //9 - Burster Enemies
    private Pooling pooledSprinterEnemies = new Pooling();   //10 - Sprinter Enemies
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
        Player = LevelManager.Instance.player;
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
        pools.Add(pooledBursterEnemies);
        pools.Add(pooledSprinterEnemies);

        SpecialEnemyChance = (100 - BasicEnemyChance) / (enemyPrefabs.Length - 1);

        IsBossAlive = false;
        SpawningWave = false;
        WaveActive = false;
        IsBossWave = false;

        SafeSpawnArea = 20;

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

        if (WaveList.Count == 0 && WaveActive)
        {
            WaveActive = false;
            if (IsBossWave)
            {
                LevelManager.Instance.SpawnAugmentChoice(); //Temp
            }
            
        }
    }

    private void SelectWave()
    {
        Wave++;
        BasicEnemyChance -= 2;
        SpecialEnemyChance = (100 - BasicEnemyChance) / (enemyPrefabs.Length - 1);

        if (Wave <= 0)
        {
            Wave = 1;
        }

        IsBossWave = false;
        if (Wave % BossWaveFrequency == 0)
        {
            IsBossWave = true;
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
        int numberToSpawn = 20 + (Wave * 2);

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
            while (true)
            {
                int random = Random.Range(0, bossPrefabs.Length);
                if (random != PreviousBoss)
                {
                    PreviousBoss = random;
                    return PreviousBoss;
                }
            }
        }
        else
        {
            float random = Random.Range(1f, 100f);

            for (int i = 1; i < enemyPrefabs.Length; i++)
            {
                if ((BasicEnemyChance + (SpecialEnemyChance * (i - 1))) < random && random <= (BasicEnemyChance + (SpecialEnemyChance * i)))
                {
                    return i;
                }
            }
        }
        return 0;
    }

    public void SpawnEnemy(bool isBoss) //Spawns Random Enemy At Random Position
    {
        if (isBoss)
        {
            HandleSpawningEnemy(true, SelectEnemyToSpawn(true), GetRandomPosition(SafeSpawnArea));
        }
        else
        {
            HandleSpawningEnemy(false, SelectEnemyToSpawn(false), GetRandomPosition(SafeSpawnArea));
        }
    }

    public void SpawnEnemy(bool isBoss, int enemyType) //Spawns Specific Enemy At Random Position
    {
        if (isBoss)
        {
            HandleSpawningEnemy(true, enemyType, GetRandomPosition(SafeSpawnArea));
        }
        else
        {
            HandleSpawningEnemy(false, enemyType, GetRandomPosition(SafeSpawnArea));
        }
    }

    public void SpawnEnemy(bool isBoss, Vector3 position) //Spawns Random Enemy At Specific Position
    {
        if (isBoss)
        {
            HandleSpawningEnemy(true, SelectEnemyToSpawn(true), position);
        }
        else
        {
            HandleSpawningEnemy(false, SelectEnemyToSpawn(true), position);
        }
    }

    public void SpawnEnemy(bool isBoss, int enemyType, Vector3 position) //Spawns Specific Enemy At Specific Position
    {
        if (isBoss)
        {
            HandleSpawningEnemy(true, enemyType, position);
        }
        else
        {
            HandleSpawningEnemy(false, enemyType, position);
        }
    }

    private void HandleSpawningEnemy(bool isBoss, int enemyType, Vector3 position)
    {
        GameObject newEnemy = null;

        if (isBoss)
        {
            newEnemy = Instantiate(bossPrefabs[enemyType], gameObject.transform);
        }
        else
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
        }

        newEnemy.transform.position = position;
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetStats();
        enemyScript.InitEnemyEffects(augmentPrefabs);
        enemyList.Add(newEnemy.transform);
        WaveList.Add(newEnemy);

        if (!WaveActive)
        {
            WaveActive = true;
        }
    }

    public Vector3 GetRandomPosition(int distanceFromPlayer)
    {
        Vector3 Top = new Vector3(PlayerPosition.x, 0, PlayerPosition.z + distanceFromPlayer);
        Vector3 Bottom = new Vector3(PlayerPosition.x, 0, PlayerPosition.z - distanceFromPlayer);
        Vector3 Right = new Vector3(PlayerPosition.x + distanceFromPlayer, 0, PlayerPosition.z);
        Vector3 Left = new Vector3(PlayerPosition.x - distanceFromPlayer, 0, PlayerPosition.z);

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

    public void RemoveFromWaveList(GameObject enemy)
    {
        WaveList.Remove(enemy);
    }
}
