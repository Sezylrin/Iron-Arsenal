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
    public BoxCollider enemyBC;
    public bool digging;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        EnemyRB = GetComponent<Rigidbody>();
        enemyBC = GetComponent<BoxCollider>();

        MaxHealth = data.maxHealth;
        DamageOnCollide = data.damageOnCollide;
        Speed = data.speed;
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

        if (Vector3.Distance(Player.transform.position, transform.position) > 10)
        {
            StartCoroutine(DigDown());
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 10)
        {
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
        Manager.PoolDiggerEnemy(gameObject);
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
        digging = true;
        //enemyBC.enabled = false;
        while (transform.localScale.y >= 1f)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.01f, transform.localScale.z);
        }
    }

    IEnumerator DigUp()
    {
        StopCoroutine(DigDown());
        while (transform.localScale.y <= 4)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.01f, transform.localScale.z);
        }
        //enemyBC.enabled = true;
        digging = false;
    }
}
