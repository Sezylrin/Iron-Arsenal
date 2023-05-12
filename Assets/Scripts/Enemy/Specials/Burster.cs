using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burster : Enemy
{
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletFireDelay { get; set; }

    public EnemyBulletData bulletData;

    public Transform enemyBulletSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyBullet;
    private Transform projectilesParent;

    void Awake()
    {
        Init();
        projectilesParent = projectilesParent = GameObject.Find("Projectiles Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();

        Move();
    }

    protected override void OnDeath()
    {
        Burst();
        base.OnDeath();
    }

    public override void SetStats(float baseHealth)
    {
        base.SetStats(baseHealth);

        BulletDamage = bulletData.damage * Mathf.Pow(1.1f, Difficulty);
        BulletSpeed = bulletData.projectileSpeed * Mathf.Pow(1.01f, Difficulty);
        BulletFireDelay = bulletData.fireDelay * Mathf.Pow(1.01f, -Difficulty);
    }

    private void Burst()
    {
        float spreadAngle = 5f;

        for (int i = 0; i < 3; i++)
        {
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

            if (spreadAngle == 0)
            {
                newEnemyBullet.transform.rotation = transform.rotation;
            }
            else newEnemyBullet.transform.rotation = transform.rotation * new Quaternion(1, 0, spreadAngle, 0);

            newEnemyBullet.transform.position = enemyBulletSpawnPoint.position;

            EnemyBasicBullet enemyBulletScript = newEnemyBullet.GetComponent<EnemyBasicBullet>();
            enemyBulletScript.SetStats(Manager, Vector3.forward, BulletDamage, BulletSpeed, BulletFireDelay);
            enemyBulletScript.Shoot();

            spreadAngle -= 5f;
        }
    }
}
