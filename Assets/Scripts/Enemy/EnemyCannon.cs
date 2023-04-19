using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float BulletFireDelay { get; set; }
    public float Wave { get; set; }

    private GameObject player;
    public StaticSentryEnemy enemy;

    public Transform rotatePoint;
    public Transform enemyBulletSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyBullet;
    private bool ableToShoot;
    private Transform projectilesParent;

    public EnemyBulletData data;
    public EnemyManager manager;

    private void Awake()
    {
        player = GameObject.Find("Player"); 
        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        projectilesParent = GameObject.Find("Projectiles Parent").transform;

        Wave = manager.wave;
        SetStats();
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Wave = manager.wave;
        Vector3 target = player.transform.position;
        target.z += 1000000;
        
        rotatePoint.LookAt(new Vector3(player.transform.position.x, enemyBulletSpawnPoint.transform.position.y, player.transform.position.z), Vector3.down);

        if (Vector3.Distance(player.transform.position, transform.position) < 12 && ableToShoot)
        {
            ableToShoot = false;

            if (enemy.Manager.pooledEnemyBullets.ListCount() > 0)
            {
                newEnemyBullet = enemy.Manager.pooledEnemyBullets.FirstObj();
                newEnemyBullet.SetActive(true);
                enemy.Manager.pooledEnemyBullets.RemoveObj(newEnemyBullet);
            }
            else
            {
                newEnemyBullet = Instantiate(enemyBullet, projectilesParent);
            }
            newEnemyBullet.transform.rotation = transform.rotation;
            newEnemyBullet.transform.position = enemyBulletSpawnPoint.position;

            EnemyBasicBullet enemyProjectileScript = newEnemyBullet.GetComponent<EnemyBasicBullet>();
            enemyProjectileScript.SetStats(manager, (target - enemyBulletSpawnPoint.position).normalized, BulletDamage, BulletSpeed, BulletFireDelay);
            enemyProjectileScript.Shoot();

            StartCoroutine(DelayFiring(enemyProjectileScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }

    public void SetStats()
    {
        BulletDamage = data.damage * Mathf.Pow(1.1f, Wave);
        BulletSpeed = data.projectileSpeed * Mathf.Pow(1.01f, Wave);
        BulletFireDelay = data.fireDelay * Mathf.Pow(1.01f, -Wave);
    }
}
