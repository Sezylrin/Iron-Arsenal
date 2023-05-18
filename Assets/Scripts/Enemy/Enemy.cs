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
        NumberManager.Instance.SpawnText(transform.position, finalDamage.ToString(), 1, Color.white);
        if (enemyEffects.isLifeSteal)
        {
            baseFunctions.RecoverHealth(finalDamage * 0.05f);
        }
        if (CurrentHealth <= 0)
        {
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

    protected virtual void OnDeath()
    {
        Manager.EnemyDeath();
        switch (type)
        {
            case EnemyType.Basic:
                Manager.PoolEnemy(gameObject, 0);
                LevelManager.Instance.GainXenorium((int)Random.Range(2, 7));
                break;
            case EnemyType.Tank:
                Manager.PoolEnemy(gameObject, 1);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Exploder:
                Manager.PoolEnemy(gameObject, 2);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Digger:
                Manager.PoolEnemy(gameObject, 3);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Charger:
                Manager.PoolEnemy(gameObject, 4);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Shooter:
                Manager.PoolEnemy(gameObject, 5);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Dodger:
                Manager.PoolEnemy(gameObject, 6);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Splitter:
                Manager.PoolEnemy(gameObject, 7);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Cloaker:
                Manager.PoolEnemy(gameObject, 8);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Burster:
                Manager.PoolEnemy(gameObject, 9);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Sprinter:
                Manager.PoolEnemy(gameObject, 10);
                LevelManager.Instance.GainXenorium((int)Random.Range(3, 8));
                break;
            case EnemyType.Boss:
                Destroy(gameObject);
                LevelManager.Instance.GainXenorium((int)Random.Range(50, 151));
                LevelManager.Instance.GainNovacite((int)Random.Range(50, 151));
                LevelManager.Instance.GainVoidStone((int)Random.Range(50, 151));
                LevelManager.Instance.SpawnAugmentChoice();
                break;
            default:
                Destroy(gameObject);
                break;
        }
        enemyEffects.fireTick = 0;
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
