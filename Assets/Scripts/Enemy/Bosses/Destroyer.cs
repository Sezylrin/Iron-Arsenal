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

    private bool canCharge;

    void Awake()
    {
        NumberOfPatterns = 4;

        Init();
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;

        StartCoroutine(DelayChoosingPattern(1));
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToShoot = true;
        canCharge = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();

        if (Vector3.Distance(Player.transform.position, transform.position) > 12)
        {
            Move();
        }

        if (ActivePattern == 0)
        {
            WaitingForNextPattern();
        }
        else PatternActivate(ActivePattern);
    }

    protected override void WaitingForNextPattern()
    {
        canCharge = true;
        base.WaitingForNextPattern();
    }

    protected override void Pattern2() //Heal Override
    {
        StartCoroutine(PatternLength(3));

        if (canHeal)
        {
            Heal(MaxHealth / 10);
            canHeal = false;
        }
    }

    protected override void Pattern3() //Mega Bomb
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

    protected override void Pattern4() //Charge
    {
        StartCoroutine(PatternLength(1));

        if (canCharge)
        {
            canCharge = false;
            EnemyRB.AddForce(Vector3.Normalize(new Vector3(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z)) * 20, ForceMode.Impulse);
        }
    }
       

    public override void SetStats(float baseHealth)
    {
        base.SetStats(baseHealth);

        BulletDamage = bulletData.damage * Mathf.Pow(1.1f, Difficulty);
        BulletSpeed = bulletData.projectileSpeed * Mathf.Pow(1.01f, Difficulty);
        BulletFireDelay = bulletData.fireDelay * Mathf.Pow(1.01f, -Difficulty);
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }
}
