using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [field: Space(15)]
    [field: SerializeField] private int NumberOfAliveEnemies { get; set; }
    [field: SerializeField] public int EnemyBaseHealth { get; private set; }
    [field: SerializeField] public int Difficulty { get; private set; }
    [field: Space(15)]
    [field: SerializeField] private int DifficultyIncreaseFrequency { get; set; }
    [field: SerializeField] private int InitialSpawnDelaySeconds { get; set; }
    [field: SerializeField] public int SafeSpawnArea { get; private set; }
    [field: Space(15)]
    [field: SerializeField, Range(0, 100)] private float BasicEnemyChance { get; set; }
    [field: SerializeField] private int BECDecreaseOnDifficultyChange { get; set; }
    [field: SerializeField] private float SpecialEnemyChance { get; set; }
    [field: Space(15)]
    [field: SerializeField] private bool IsRushActive { get; set; }
    [field: SerializeField] private int NumberOfSpawnsThisRush { get; set; }
    public bool debugStartRush;
    public bool debugEndRush;
    [field: Space(15)]
    [field: SerializeField] public bool IsBossAlive { get; private set; }
    [field: SerializeField] public GameObject ActiveBoss { get; private set; }
    [field: SerializeField] private int PreviousBoss { get; set; }
    [field: SerializeField] private bool IsBossRushActive { get; set; }
    [field: SerializeField] private int BossesDefeatedInRush { get; set; }
    public bool debugStartBossRush;
    [field: Space(15)]
    [field: SerializeField] private float NormalSpawnDelay { get; set; }
    [field: SerializeField] private float NSDMinimum { get; set; }
    [field: SerializeField] private float NSDMultiplier { get; set; }
    [field: SerializeField] private float NSDExponential { get; set; }
    [field: Space(15)]
    [field: SerializeField] private float RushSpawnDelay { get; set; }
    [field: SerializeField] private float RSDMinimum { get; set; }
    [field: SerializeField] private float RSDMultiplier { get; set; }
    [field: SerializeField] private float RSDSpawnsModifier { get; set; }
    [field: SerializeField] private float RSDExponential { get; set; }
    [field: Space(15)]
    [field: SerializeField] private int AllowedEnemies { get; set; }
    [field: SerializeField] private int AEMin { get; set; }
    [field: SerializeField] private int AEDifficultyModifier { get; set; }
    [field: SerializeField] private int AERushBonus { get; set; }
    [field: Space(15)]
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
    private Pooling pooledSprinterEnemies = new Pooling();  //3 - Sprinter Enemies
    private Pooling pooledChargerEnemies = new Pooling();   //4 - Charger Enemies
    private Pooling pooledSentryEnemies = new Pooling();    //5 - Sentry Enemies
    private Pooling pooledDodgerEnemies = new Pooling();    //6 - Dodger Enemies
    private Pooling pooledSplitterEnemies = new Pooling();  //7 - Splitter Enemies
    private Pooling pooledBursterEnemies = new Pooling();   //8 - Cloaker Enemies
    //private Pooling pooledBursterEnemies = new Pooling();    //9 - Burster Enemies
    //private Pooling pooledSprinterEnemies = new Pooling();   //10 - Sprinter Enemies
    public List<Pooling> enemyPools = new List<Pooling>();

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
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = LevelManager.Instance.player;

        enemyPools.Add(pooledBasicEnemies);
        enemyPools.Add(pooledTankEnemies);
        enemyPools.Add(pooledExploderEnemies);
        enemyPools.Add(pooledSprinterEnemies);
        enemyPools.Add(pooledChargerEnemies);
        enemyPools.Add(pooledSentryEnemies);
        enemyPools.Add(pooledDodgerEnemies);
        enemyPools.Add(pooledSplitterEnemies);
        enemyPools.Add(pooledBursterEnemies);

        IsBossAlive = false;
        IsRushActive = false;
        IsBossRushActive = false;

        PreviousBoss = -1;
        debugStartRush = false;
        debugEndRush = false;
        debugStartBossRush = false;

        if (Difficulty <= 0)
        {
            Difficulty = 1;
        }

        SpecialEnemyChance = (100 - BasicEnemyChance) / (enemyPrefabs.Length - 1);

        SetMaxAllowedEnemies(false);
        SetNormalSpawnDelay();
        SetRushSpawnDelay();

        StartCoroutine(InitialSpawnDelay());
        StartCoroutine(IncreaseDifficulty());
        StartCoroutine(Rush());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = Player.transform.position;

        if (debugStartRush)
        {
            debugStartRush = false;
            StartRush();
        }
        if (debugEndRush)
        {
            debugEndRush = false;
            StopRush();
        }
        if (debugStartBossRush)
        {
            debugStartBossRush = false;
            StartBossRush();
        }
    }

    private void SetNormalSpawnDelay() //Warning: Math
    {
        NormalSpawnDelay = NSDMinimum + NSDMultiplier * (Mathf.Pow(NSDExponential, 1 - Difficulty));
    }

    private void SetRushSpawnDelay() //Warning: Math
    {
        RushSpawnDelay = RSDMinimum + ((RSDMultiplier - (RSDSpawnsModifier * NumberOfSpawnsThisRush)) * (Mathf.Pow(RSDExponential, (1 - Difficulty))));
    }

    private void SetMaxAllowedEnemies(bool IsRushStart) //Warning: Math
    {
        if (IsRushStart)
        {
            AllowedEnemies = AEMin + AERushBonus + ((Difficulty - 1) * AEDifficultyModifier);
        }
        else AllowedEnemies = AEMin + ((Difficulty - 1) * AEDifficultyModifier);
    }

    private void UpdateSpawnChances()
    {
        BasicEnemyChance -= BECDecreaseOnDifficultyChange;
        SpecialEnemyChance = (100 - BasicEnemyChance) / (enemyPrefabs.Length - 1);
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(DifficultyIncreaseFrequency);
            Difficulty++;
            UpdateSpawnChances();

            if (IsRushActive)
            {
                SetMaxAllowedEnemies(true);
            }
            else SetMaxAllowedEnemies(false);
        }
    }

    private IEnumerator InitialSpawnDelay()
    {
        yield return new WaitForSeconds(InitialSpawnDelaySeconds);
        StartCoroutine(DelaySpawn());
    }

    private IEnumerator DelaySpawn()
    {
        while (true)
        {
            SetNormalSpawnDelay();
            yield return new WaitForSeconds(NormalSpawnDelay);
            if (NumberOfAliveEnemies < AllowedEnemies)
            {
                SpawnEnemy(false);
            }
        }
    }

    private IEnumerator Rush()
    {
        while (true)
        {
            SetRushSpawnDelay();
            yield return new WaitForSeconds(RushSpawnDelay);
            if (IsRushActive && NumberOfAliveEnemies < AllowedEnemies)
            {
                SpawnEnemy(false);
                NumberOfSpawnsThisRush++;
            }
        }
    }

    private IEnumerator BossRush()
    {
        int bossesSpawned = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (bossesSpawned != bossPrefabs.Length && !IsBossAlive)
            {
                SpawnEnemy(true, bossesSpawned);
                bossesSpawned++;
            }

            if (BossesDefeatedInRush >= bossPrefabs.Length)
            {
                break;
            }
        }
        GameManager.Instance.HandleVictory();
    }

    public void StartRush()
    {
        StopCoroutine(RushTimer(0));
        IsRushActive = true;
        NumberOfSpawnsThisRush = 0;
        SetMaxAllowedEnemies(true);
    }

    public void StartRushWithTimer(int lengthInSeconds)
    {
        StopCoroutine(RushTimer(0));
        IsRushActive = true;
        NumberOfSpawnsThisRush = 0;
        SetMaxAllowedEnemies(true);
        StartCoroutine(RushTimer(lengthInSeconds));
    }

    public IEnumerator RushTimer(int lengthInSeconds)
    {
        yield return new WaitForSeconds(lengthInSeconds);
        StopRush();
    }

    public void StopRush()
    {
        StopCoroutine(RushTimer(0));
        IsRushActive = false;
        SetMaxAllowedEnemies(false);
    }

    public void StartBossRush()
    {
        IsBossRushActive = true;
        StartCoroutine(BossRush());
    }

    public void EnemyDeath()
    {
        NumberOfAliveEnemies--;
    }

    public void BossDeath(Transform bossTransform)
    {
        enemyList.Remove(bossTransform);
        ActiveBoss = null;
        IsBossAlive = false;
        if (IsBossRushActive)
        {
            BossesDefeatedInRush++;
        }
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
            ActiveBoss = newEnemy;
            IsBossAlive = true;
        }
        else
        {
            if (enemyPools[enemyType].ListCount() > 0)
            {
                newEnemy = enemyPools[enemyType].FirstObj();
                newEnemy.SetActive(true);
                enemyPools[enemyType].RemoveObj(newEnemy);
            }
            else
            {
                newEnemy = Instantiate(enemyPrefabs[enemyType], gameObject.transform);
            }
        }

        newEnemy.transform.position = position;
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetStats(EnemyBaseHealth);
        enemyScript.InitEnemyEffects(augmentPrefabs);
        enemyList.Add(newEnemy.transform);
        NumberOfAliveEnemies++;
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
        enemyPools[enemyType].AddObj(obj);
    }

    public void PoolEnemyBullet(GameObject obj)
    {
        obj.SetActive(false);
        pooledEnemyBullets.AddObj(obj);
    }
}
