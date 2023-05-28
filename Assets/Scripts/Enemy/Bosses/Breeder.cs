using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breeder : Boss
{
    [field: Header("Enemy Spawning")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public bool canSpawnBasic;
    public bool canSpawnSpecial;

    protected override void Awake()
    {
        NumberOfPatterns = 6;

        Init();

        StartCoroutine(DelayChoosingPattern(1));
    }

    // Start is called before the first frame update
    void Start()
    {
        canSpawnBasic = true;
        canSpawnSpecial = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckEffectState();
        SetRotation();

        if (Vector3.Distance(Player.transform.position, transform.position) > 12)
        {
            Move();
        }

        if (canSpawnBasic)
        {
            canSpawnBasic = false;
            Manager.SpawnEnemy(false, 0, spawnPoint1.position);
            StartCoroutine(DelayBasicSpawn(8));
        }

        if (ActivePattern == 0)
        {
            WaitingForNextPattern();
        }
        else PatternActivate(ActivePattern);
    }

    protected override void Pattern1() //Move and Melee Override
    {
        StartCoroutine(PatternLength(3));

        if (Vector3.Distance(Player.transform.position, transform.position) > 8)
        {
            Move();
        }
    }

    protected override void Pattern3() //Exploder Rush
    {
        StartCoroutine(PatternLength(14));

        if (canSpawnSpecial)
        {
            canSpawnSpecial = false;

            Manager.SpawnEnemy(false, 2, spawnPoint2.position);
            Manager.SpawnEnemy(false, 2, spawnPoint3.position);

            StartCoroutine(DelaySpecialSpawn(7));
        }
    }

    protected override void Pattern4() //Splitter Rush
    {
        StartCoroutine(PatternLength(14));

        if (canSpawnSpecial)
        {
            canSpawnSpecial = false;

            Manager.SpawnEnemy(false, 7, spawnPoint2.position);
            Manager.SpawnEnemy(false, 7, spawnPoint3.position);

            StartCoroutine(DelaySpecialSpawn(7));
        }
    }

    protected override void Pattern5() //Dodger Rush
    {
        StartCoroutine(PatternLength(14));

        if (canSpawnSpecial)
        {
            canSpawnSpecial = false;

            Manager.SpawnEnemy(false, 6, spawnPoint2.position);
            Manager.SpawnEnemy(false, 6, spawnPoint3.position);

            StartCoroutine(DelaySpecialSpawn(7));
        }
    }

    protected override void Pattern6() //Random Rush
    {
        StartCoroutine(PatternLength(16));

        if (canSpawnSpecial)
        {
            canSpawnSpecial = false;

            Manager.SpawnEnemy(false, spawnPoint2.position);
            Manager.SpawnEnemy(false, spawnPoint3.position);
            
            StartCoroutine(DelaySpecialSpawn(8));
        }
    }

    private IEnumerator DelayBasicSpawn(int delay)
    {
        yield return new WaitForSeconds(delay);
        canSpawnBasic = true;
    }

    private IEnumerator DelaySpecialSpawn(int delay)
    {
        yield return new WaitForSeconds(delay);
        canSpawnSpecial = true;
    }
}
