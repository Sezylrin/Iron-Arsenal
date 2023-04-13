using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Basic,
    Tank,
    Exploder,
    Digger,
    Charger,
    Shooter,
    Dodger,
    Splitter,
    Cloaker,
    Enemy9,
    Enemy10,
    StaticSentry
};

public abstract class Enemy : MonoBehaviour
{
    public EnemyType type;

    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }
    public Rigidbody EnemyRB { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float DamageOnCollide { get; set; }
    public float Speed { get; set; }
    public int RamLaunchMultiplier { get; set; }

    public EnemyData data;

    protected virtual void Init()
    {
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        EnemyRB = GetComponent<Rigidbody>();

        SetStats(Manager.wave);
        CurrentHealth = MaxHealth;
    }

    public virtual void SetStats(int wave)
    {
        MaxHealth = data.maxHealth * Mathf.Pow(1.1f, wave - 1);
        DamageOnCollide = data.damageOnCollide * Mathf.Pow(1.1f, wave - 1);
        Speed = data.speed * Mathf.Pow(1.005f, wave - 1);
        RamLaunchMultiplier = data.ramLaunchMultiplier;
    }

    protected virtual void SetRotation()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
    }

    protected virtual void Move()
    {
        Vector3 direction = Vector3.forward;
        transform.Translate(direction.x * Speed * Time.deltaTime, 0, direction.z * Speed * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            EnemyRB.AddForce(Vector3.Normalize(new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z)) * RamLaunchMultiplier, ForceMode.Impulse);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            StopCoroutine(TakeDamageOverTime(0f));
            OnDeath();
        }
    }

    public virtual void StartDamageOverTime(float damage)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(TakeDamageOverTime(damage));
        }
    }

    public IEnumerator TakeDamageOverTime(float damage)
    {
        int ticks = 0;
        while (ticks < 5)
        {
            yield return new WaitForSeconds(1f);
            TakeDamage(damage);
            ticks++;
        }
    }

    protected virtual void OnDeath()
    {
        switch (type)
        {
            case EnemyType.Basic:
                Manager.PoolEnemy(gameObject, 0);
                break;
            case EnemyType.Tank:
                Manager.PoolEnemy(gameObject, 1);
                break;
            case EnemyType.Exploder:
                Manager.PoolEnemy(gameObject, 2);
                break;
            case EnemyType.Digger:
                Manager.PoolEnemy(gameObject, 3);
                break;
            case EnemyType.Charger:
                Manager.PoolEnemy(gameObject, 4);
                break;
            case EnemyType.Shooter:
                Manager.PoolEnemy(gameObject, 5);
                break;
            case EnemyType.Dodger:
                Manager.PoolEnemy(gameObject, 6);
                break;
            case EnemyType.Splitter:
                Manager.PoolEnemy(gameObject, 7);
                break;
            case EnemyType.Cloaker:
                Manager.PoolEnemy(gameObject, 8);
                break;
        }
    }

    public virtual void StartSlow(float slowStrength)
    {
        if (gameObject.activeSelf)
        {
            StopCoroutine(SlowEnemy(slowStrength));
            StartCoroutine(SlowEnemy(slowStrength));
        }
    }
            
    public virtual IEnumerator SlowEnemy(float slowStrength)
    {
        Speed = (data.speed * Mathf.Pow(1.005f, data.wave - 1)) * slowStrength;
        yield return new WaitForSeconds(5f);
        Speed = data.speed * Mathf.Pow(1.005f, data.wave - 1);
    }
}
