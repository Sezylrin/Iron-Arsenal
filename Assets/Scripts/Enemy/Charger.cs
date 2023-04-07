using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour, IEnemy
{
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }

    public EnemyData data;

    private bool ableToCharge;
    private Vector3 chargeDirection;

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
        ableToCharge = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        Vector3 direction = Vector3.forward;

        if (Vector3.Distance(Player.transform.position, transform.position) > 15)
        {
            transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && ableToCharge)
        {
            ableToCharge = false;
            StartCoroutine(StartChargingUp());
        }

        if (Vector3.Distance(Player.transform.position, transform.position) > 20)
        {
            StopCoroutine(StartChargingUp());
            ableToCharge = true;
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
        Manager.PoolChargerEnemy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            EnemyRB.AddForce((transform.position - col.transform.position).normalized * 750);
        }
    }

    IEnumerator StartChargingUp()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        chargeDirection = (Player.transform.position - transform.position).normalized;
        EnemyRB.AddForce(chargeDirection * 1000);
        yield return new WaitForSeconds(3);
        ableToCharge = true;
    }
}
