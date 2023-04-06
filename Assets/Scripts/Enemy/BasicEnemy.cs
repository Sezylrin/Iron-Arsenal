using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public float MaxHealth { get; set; } = 100;
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; } = 3;
    public float Speed { get; set; } = 3f;

    public Rigidbody enemyRB;
    

    void Awake()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        enemyRB = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) > 10)
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0 ) 
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Manager.PoolBasicEnemy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            enemyRB.AddForce((transform.position - col.transform.position).normalized * 750);
        }
    }
}
