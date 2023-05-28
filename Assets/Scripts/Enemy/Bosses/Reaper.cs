using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : Boss
{
    [field: Header("Projectile Stats")]
    [field: SerializeField] public float BulletDamage { get; set; }
    [field: SerializeField] public float BulletSpeed { get; set; }
    [field: SerializeField] public float BulletFireDelay { get; set; }

    [field: Header("Shooting Burst")]
    [field: SerializeField] private bool ableToShoot;
    public EnemyBulletData bulletData;
    public Transform reaperBulletSpawnPoint;
    public GameObject reaperBulletPrefab;

    private GameObject newReaperBullet;
    private Transform projectilesParent;

    [field: Header("Phazing")]
    [field: SerializeField] private bool phazing;
    public BoxCollider enemyBC;
    public MeshRenderer[] renderers;
    public Material opaqueMaterial;
    public Material transparentMaterial;
    

    [field: Header("Flying Burst")]
    [field: SerializeField] private bool canDash;

    [field: Header("Teleporting")]
    [field: SerializeField] private bool teleporting;
    public Material invisibleMaterial;



    protected override void Awake()
    {
        NumberOfPatterns = 6;

        Init();
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;
        enemyBC = GetComponent<BoxCollider>();

        StartCoroutine(DelayChoosingPattern(1));  
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToShoot = true;
        phazing = false;
        canDash = true;
        teleporting = false;
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

        if (ActivePattern == 0)
        {
            WaitingForNextPattern();
        }
        else PatternActivate(ActivePattern);
    }

    protected override void WaitingForNextPattern()
    {
        if (phazing)
        {
            phazing = false;
            enemyBC.enabled = true;

            ChangeMaterials(opaqueMaterial);
        }

        if (teleporting)
        {
            teleporting = false;
        }
        base.WaitingForNextPattern();
    }

    protected override void Pattern3() //Shooting Burst
    {
        StartCoroutine(PatternLength(5));

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && ableToShoot)
        {
            ableToShoot = false;

            newReaperBullet = Instantiate(reaperBulletPrefab, projectilesParent);

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

    protected override void Pattern4() //Phazing
    {
        StartCoroutine(PatternLength(8));

        if (!phazing)
        {
            phazing = true;
            enemyBC.enabled = false;

            ChangeMaterials(transparentMaterial);
        }
    }

    protected override void Pattern5() //Dash Burst
    {
        StartCoroutine(PatternLength(7));

        if (canDash)
        {
            canDash = false;

            EnemyRB.AddForce(Vector3.Normalize(new Vector3(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z)) * 25, ForceMode.Impulse);

            StartCoroutine(DelayDashBurst(3f));
        }
    }
    protected override void Pattern6() //Teleport
    {
        StartCoroutine(PatternLength(7));
        if (!teleporting)
        {
            teleporting = true;
            StartCoroutine(Teleport());
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

    IEnumerator DelayDashBurst(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDash = true;
    }

    IEnumerator Teleport()
    {
        ChangeMaterials(transparentMaterial);
        yield return new WaitForSeconds(2);

        ChangeMaterials(invisibleMaterial);
        yield return new WaitForSeconds(2);

        transform.position = Manager.GetRandomPosition(15);
        ChangeMaterials(transparentMaterial);
        yield return new WaitForSeconds(2);

        ChangeMaterials(opaqueMaterial);
    }

    private void ChangeMaterials(Material material)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = material;
        }
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            BaseFunctions tempBase = col.gameObject.GetComponent<BaseFunctions>();
            TakeDamage(StatsManager.Instance.healthFactor * tempBase.collisionFactor);
            tempBase.TakeDamage(DamageOnCollide);
            EnemyRB.AddForce(Vector3.Normalize(new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z)) * RamLaunchMultiplier, ForceMode.Impulse);
            Heal(DamageOnCollide / 2);
        }
    }
}