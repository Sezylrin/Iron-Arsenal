using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [field: Header("Bullet")]
    [field: SerializeField] public float BulletDamage { get; set; }
    [field: SerializeField] public float BulletSpeed { get; set; }
    [field: SerializeField] public float BulletFireDelay { get; set; }

    public EnemyBulletData bulletData;

    public Transform enemyBulletSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyBullet;
    private bool ableToShoot;
    private Transform projectilesParent;

    void Awake()
    {
        Init();
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;
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

        if (Vector3.Distance(Player.transform.position, transform.position) > 10)
        {
            Move();
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 12 && ableToShoot)
        {
            ableToShoot = false;

            if (Manager.pooledEnemyBullets.ListCount() > 0)
            {
                newEnemyBullet = Manager.pooledEnemyBullets.FirstObj();
                newEnemyBullet.SetActive(true);
                Manager.pooledEnemyBullets.RemoveObj(newEnemyBullet);
            }
            else
            {
                newEnemyBullet = Instantiate(enemyBullet, projectilesParent);
            }
            newEnemyBullet.transform.rotation = transform.rotation;
            newEnemyBullet.transform.position = enemyBulletSpawnPoint.position;

            EnemyBasicBullet enemyBulletScript = newEnemyBullet.GetComponent<EnemyBasicBullet>();
            enemyBulletScript.SetStats(Manager, Vector3.forward, BulletDamage, BulletSpeed, BulletFireDelay);
            enemyBulletScript.Shoot();

            StartCoroutine(DelayFiring(enemyBulletScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }

    protected override void OnDeath()
    {
        StopCoroutine(DelayFiring(0f));
        ableToShoot = true;
        base.OnDeath();
    }

    public override void SetStats(float baseHealth)
    {
        base.SetStats(baseHealth);

        BulletDamage = bulletData.damage * Mathf.Pow(1.1f, Difficulty);
        BulletSpeed = bulletData.projectileSpeed * Mathf.Pow(1.01f, Difficulty);
        BulletFireDelay = bulletData.fireDelay * Mathf.Pow(1.01f, -Difficulty);
    }
}
