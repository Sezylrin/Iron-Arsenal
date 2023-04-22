using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : Boss
{
    
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletFireDelay { get; set; }

    public EnemyBulletData bulletData;

    public Transform destroyerBombSpawnPoint;
    public GameObject destroyerBomb;

    private GameObject newDestroyerBomb;
    private bool ableToShoot;
    private Transform projectilesParent;

    void Awake()
    {
        Init();
        NumberOfPatterns = 2;
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;

        StartCoroutine(DelayChoosingPattern(1));
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();

        if (Vector3.Distance(Player.transform.position, transform.position) > 12)
        {
            Move();
        }

        else if (ActivePattern == 1)
        {
            Pattern1();
        }
        else if (ActivePattern == 2)
        {
            Pattern2();
        }
        else if (ActivePattern == 3)
        {
            Pattern3();
        }
        else if (ActivePattern == 4)
        {
            Pattern4();
        }
    }

    private void Pattern1() //Mega Bomb
    {
        StartCoroutine(PatternLength(1));

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && ableToShoot)
        {
            ableToShoot = false;

            newDestroyerBomb = Instantiate(destroyerBomb, projectilesParent);

            newDestroyerBomb.transform.rotation = transform.rotation;
            newDestroyerBomb.transform.position = destroyerBombSpawnPoint.position;

            DestroyerBomb destroyerBombScript = newDestroyerBomb.GetComponent<DestroyerBomb>();
            destroyerBombScript.SetStats(this, Vector3.forward, BulletDamage, BulletSpeed, BulletFireDelay);
            destroyerBombScript.Shoot();

            StartCoroutine(DelayFiring(destroyerBombScript.FireDelay));
        }

    }

    private void Pattern2() //Bash
    {
        StartCoroutine(PatternLength(10));

        Move();
    }

    private void Pattern3() //Heal
    { 
        StartCoroutine(PatternLength(3));

        Heal(MaxHealth / 10);
    }

    private void Pattern4()
    {
        StartCoroutine(PatternLength(10));

    }

    public override void SetStats()
    {
        base.SetStats();

        BulletDamage = bulletData.damage * Mathf.Pow(1.1f, Wave);
        BulletSpeed = bulletData.projectileSpeed * Mathf.Pow(1.01f, Wave);
        BulletFireDelay = bulletData.fireDelay * Mathf.Pow(1.01f, -Wave);
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }
}
