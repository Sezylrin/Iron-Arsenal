using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : Boss
{
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletFireDelay { get; set; }

    public EnemyBulletData bulletData;

    public Transform reaperBulletSpawnPoint;
    public GameObject reaperBullet;

    private GameObject newReaperBullet;
    private bool ableToShoot;
    private Transform projectilesParent;

    public BoxCollider enemyBC;

    public MeshRenderer[] renderers;
    public Material opaqueMaterial;
    public Material transparentMaterial;

    private bool phazing;

    void Awake()
    {
        Init();
        NumberOfPatterns = 2;
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;
        enemyBC = GetComponent<BoxCollider>();

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

        if (ActivePattern == 0)
        {
            if (phazing)
            {
                phazing = false;
                enemyBC.enabled = true;

                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material = opaqueMaterial;
                }
            }
            return;
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

    private void Pattern1() //Shooting Burst
    {
        StartCoroutine(PatternLength(3));

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && ableToShoot)
        {
            ableToShoot = false;

            newReaperBullet = Instantiate(reaperBullet, projectilesParent);

            int random = Random.Range(-10, 11);
            if ( -1 <= random && random <= 1)
            {
                newReaperBullet.transform.rotation = transform.rotation;
            }
            else newReaperBullet.transform.rotation = transform.rotation * new Quaternion(1, 0, random, 0);
            newReaperBullet.transform.position = reaperBulletSpawnPoint.position;

            EnemyReaperBullet reaperBulletScript = newReaperBullet.GetComponent<EnemyReaperBullet>();
            reaperBulletScript.SetStats(this, Vector3.forward, BulletDamage, BulletSpeed, BulletFireDelay);
            reaperBulletScript.Shoot();

            StartCoroutine(DelayFiring(reaperBulletScript.FireDelay));
        }
    }

    private void Pattern2() //Phazing
    {
        StartCoroutine(PatternLength(10));

        if (!phazing)
        {
            phazing = true;
            enemyBC.enabled = false;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = transparentMaterial;
            }
        }
        
    }

    private void Pattern3() //Rush
    {
        StartCoroutine(PatternLength(10));

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

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            Heal(DamageOnCollide / 10);
            EnemyRB.AddForce(Vector3.Normalize(new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z)) * RamLaunchMultiplier, ForceMode.Impulse);
        }
    }
}