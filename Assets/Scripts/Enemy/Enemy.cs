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
    Burster,
    Sprinter,
    StaticSentry,
    Boss
};

public abstract class Enemy : MonoBehaviour
{
    [field: Header("Stats")]
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float DamageOnCollide { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public int RamLaunchMultiplier { get; set; }

    [field: Header("Enemy Other")]
    public EnemyType type;
    [field: SerializeField] public Rigidbody EnemyRB { get; set; }
    public EnemyData data;
    [field: SerializeField] public int Difficulty { get; set; }
    public EnemyManager Manager { get; set; }
    public GameObject Player { get; set; }

    public EnemyEffects enemyEffects;

    public float damageFactor = 1;

    public float speedFactor = 1;

    private BaseFunctions baseFunctions;

    protected virtual void Init()
    {
        Player = GameObject.Find("Player");
        baseFunctions = Player.GetComponent<BaseFunctions>();
        Manager = EnemyManager.Instance;
        Difficulty = Manager.Difficulty;
        SetStats(Manager.EnemyBaseHealth);
    }

    public virtual void SetStats(float baseHealth)
    {
        MaxHealth = data.HealthScale * baseHealth * Mathf.Pow(1.15f, Difficulty - 1);
        DamageOnCollide = data.damageOnCollide * Mathf.Pow(1.1f, Difficulty - 1);
        Speed = data.speed * Mathf.Pow(1.005f, Difficulty - 1);
        RamLaunchMultiplier = data.ramLaunchMultiplier;
        CurrentHealth = MaxHealth;
    }

    protected virtual void SetRotation()
    {
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
    }

    protected virtual void Move()
    { 
        transform.Translate(Vector3.forward * Speed * speedFactor * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            BaseFunctions tempBase = col.gameObject.GetComponent<BaseFunctions>();
            TakeDamage(StatsManager.Instance.healthFactor * tempBase.collisionFactor);
            tempBase.TakeDamage(DamageOnCollide);
            EnemyRB.AddForce(Vector3.Normalize(new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z)) * RamLaunchMultiplier, ForceMode.Impulse);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
            return;
        if (type.Equals(EnemyType.Boss) && enemyEffects.isBossDamage)
            damage *= 1.2f;
        float finalDamage = damage * damageFactor;
        CurrentHealth -= finalDamage;
        if (enemyEffects.isLifeSteal)
        {
            baseFunctions.RecoverHealth(finalDamage * 0.05f);
        }
        if (CurrentHealth <= 0)
        {
            StopCoroutine(TakeDamageOverTime(0f));
            
            OnDeath();
            if (enemyEffects.isExplode && !type.Equals(EnemyType.Exploder))
            {
                Explosion tempExplosion = Instantiate(enemyEffects.augmentPFList[0], transform.position, Quaternion.identity).GetComponent<Explosion>();
                tempExplosion.SetDamage(MaxHealth * 0.3f);
            }
            if (enemyEffects.isFreezeShard)
            {
                enemyEffects.ReleaseShard();
            }
            if (enemyEffects.isShieldSteal)
            {
                baseFunctions.DecreaseRecovery();
            }
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
        Manager.EnemyDeath();
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
            case EnemyType.Burster:
                Manager.PoolEnemy(gameObject, 8);
                break;
            case EnemyType.Sprinter:
                Manager.PoolEnemy(gameObject, 8);
                break;
            default:
                Destroy(gameObject);
                break;
        }
        enemyEffects.fireTick = 0;
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
        Speed = (data.speed * Mathf.Pow(1.005f, Difficulty - 1)) * slowStrength;
        yield return new WaitForSeconds(5f);
        Speed = data.speed * Mathf.Pow(1.005f, Difficulty - 1);
    }

    public void InitEnemyEffects(GameObject[] augmentPF)
    {
        if (!enemyEffects)
        {
            enemyEffects = gameObject.AddComponent<EnemyEffects>();
            enemyEffects.hostenemy = this;
        }
        enemyEffects.SetAugmentState();
        enemyEffects.augmentPFList = augmentPF;
        
    }
}
