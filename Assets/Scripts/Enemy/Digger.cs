using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }

    public EnemyData data;

    private BoxCollider enemyBC;
    private bool digging;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        EnemyRB = GetComponent<Rigidbody>();
        enemyBC = GetComponent<BoxCollider>();

        SetStats(Manager.wave);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        digging = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        Vector3 direction = Vector3.forward;
        transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);

        if (Vector3.Distance(Player.transform.position, transform.position) > 15 && !digging)
        {
            digging = true;
            StartCoroutine(DigDown());
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 10 && digging)
        {
            digging = false;
            StartCoroutine(DigUp());
        }
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
        Manager.PoolEnemy(gameObject, 3);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            EnemyRB.AddForce((transform.position - col.transform.position).normalized * 750);
        }
    }

    IEnumerator DigDown()
    {
        StopCoroutine(DigUp());
        enemyBC.enabled = false;
        while (transform.position.y >= -0.5) 
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, -0.02f, 0);
        }
    }

    IEnumerator DigUp()
    {
        StopCoroutine(DigDown());
        while (transform.position.y <= 1)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, 0.02f, 0);
        }
        enemyBC.enabled = true;
    }

    public void SetStats(int wave)
    {
        MaxHealth = data.maxHealth * Mathf.Pow(1.1f, wave);
        DamageOnCollide = data.damageOnCollide * Mathf.Pow(1.1f, wave);
        Speed = data.speed * Mathf.Pow(1.005f, wave);
    }
}
