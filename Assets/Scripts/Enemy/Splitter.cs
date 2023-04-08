using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }

    public EnemyData data;

    public GameObject basicEnemy;

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        Vector3 direction = Vector3.forward;
        transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);
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
        int numberToSpawn = 2;
        GameObject newEnemy = null;
        int positionModifier = -1;

        while (numberToSpawn > 0)
        {
            if (Manager.pools[0].ListCount() > 0)
            {
                newEnemy = Manager.pools[0].FirstObj();
                newEnemy.SetActive(true);
                Manager.pools[0].RemoveObj(newEnemy);
            }
            else
            {
                newEnemy = Instantiate(basicEnemy, Manager.transform);
            }
            IEnemy enemyScript = newEnemy.GetComponent<IEnemy>();
            enemyScript.CurrentHealth = enemyScript.MaxHealth;

            newEnemy.transform.position = new Vector3(transform.position.x + positionModifier, transform.position.y, transform.position.z + positionModifier);

            positionModifier += 2;
            numberToSpawn--;
        } 

        Manager.PoolEnemy(gameObject, 7);
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
