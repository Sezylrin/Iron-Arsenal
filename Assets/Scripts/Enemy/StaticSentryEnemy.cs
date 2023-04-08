using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSentryEnemy : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }

    public EnemyData data;

    private EnemyCannon enemyCannonScript;

    public GameObject explosion;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        EnemyRB = GetComponent<Rigidbody>();

        MaxHealth = data.maxHealth;
        DamageOnCollide = data.damageOnCollide;
        Speed = data.speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

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
        Instantiate(explosion, new Vector3(transform.position.x, -2, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
        }
    }

    public void SetStats(int wave)
    {
        MaxHealth = data.maxHealth * Mathf.Pow(1.1f, wave);
        DamageOnCollide = data.damageOnCollide * Mathf.Pow(1.1f, wave);
        Speed = data.speed * Mathf.Pow(1.005f, wave);
    }
}

