using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    private GameObject player;
    public StaticSentryEnemy enemy;

    public Transform rotatePoint;
    public Transform enemyBulletSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyBullet;
    private bool ableToShoot;
    private Transform projectilesParent;

    private void Awake()
    {
        player = GameObject.Find("Player"); 
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

            EnemyBullet enemyProjectileScript = newEnemyBullet.GetComponent<EnemyBullet>();
            enemyProjectileScript.Direction = (target - enemyBulletSpawnPoint.position).normalized;
            enemyProjectileScript.Owner = enemy;
            enemyProjectileScript.Shoot();

            StartCoroutine(DelayFiring(enemyProjectileScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }
}
