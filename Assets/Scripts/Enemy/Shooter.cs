using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooter : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }

    public EnemyData data;

    public Transform enemyBulletSpawnPoint;
    public GameObject enemyBullet;

    private GameObject newEnemyBullet;
    private bool ableToShoot;
    private Transform projectilesParent;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        EnemyRB = GetComponent<Rigidbody>();

        SetStats(Manager.wave);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        ableToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        if (Vector3.Distance(Player.transform.position, transform.position) > 10)
        {
            Vector3 direction = Vector3.forward;
            transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);
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

            EnemyBullet enemyBulletScript = newEnemyBullet.GetComponent<EnemyBullet>();
            enemyBulletScript.Direction = Vector3.forward;
            enemyBulletScript.Owner = this;
            enemyBulletScript.Shoot();

            StartCoroutine(DelayFiring(enemyBulletScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        StopCoroutine(DelayFiring(0f));
        ableToShoot = true;
        Manager.PoolEnemy(gameObject, 5);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            EnemyRB.AddForce((transform.position - col.transform.position).normalized * 750);
        }
    }

    public void SetStats(int wave)
    {
        MaxHealth = data.maxHealth * Mathf.Pow(1.1f, wave);
        DamageOnCollide = data.damageOnCollide * Mathf.Pow(1.1f, wave);
        Speed = data.speed * Mathf.Pow(1.005f, wave);
    }
}
