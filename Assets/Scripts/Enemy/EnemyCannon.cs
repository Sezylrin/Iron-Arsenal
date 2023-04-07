using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    private GameObject player;
    public GameObject enemy;
    public IEnemy enemyScript;

    public Transform rotatePoint;
    public Transform enemyProjectileSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyProjectile;
    private bool ableToShoot;
    private Transform projectilesParent;

    private Pooling pooledEnemyBullets = new Pooling();

    private void Awake()
    {
        player = GameObject.Find("Player");
        enemyScript = enemy.GetComponent<IEnemy>();
        projectilesParent = GameObject.Find("Projectiles Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = player.transform.position;
        target.z += 1000000;
        
        rotatePoint.LookAt(new Vector3(player.transform.position.x, enemyProjectileSpawnPoint.transform.position.y, player.transform.position.z), Vector3.down);

        if (Vector3.Distance(player.transform.position, transform.position) < 12 && ableToShoot)
        {
            ableToShoot = false;

            if (pooledEnemyBullets.ListCount() > 0)
            {
                newEnemyProjectile = pooledEnemyBullets.FirstObj();
                newEnemyProjectile.SetActive(true);
                pooledEnemyBullets.RemoveObj(newEnemyProjectile);
            }
            else
            {
                newEnemyProjectile = Instantiate(enemyBullet, projectilesParent);
            }
            newEnemyProjectile.transform.rotation = transform.rotation;
            newEnemyProjectile.transform.position = enemyProjectileSpawnPoint.position;

            EnemyBullet enemyProjectileScript = newEnemyProjectile.GetComponent<EnemyBullet>();
            enemyProjectileScript.Direction = (target - enemyProjectileSpawnPoint.position).normalized;
            enemyProjectileScript.Owner = this;
            enemyProjectileScript.Shoot();

            StartCoroutine(DelayFiring(enemyProjectileScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }

    public void PoolEnemyBullet(GameObject obj)
    {
        obj.SetActive(false);
        pooledEnemyBullets.AddObj(obj);
    }
}
