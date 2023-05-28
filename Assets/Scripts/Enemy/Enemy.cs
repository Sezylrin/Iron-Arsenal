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

    public AudioSource damageAudio;
    public GameObject deathAudio;

    public ParticleSystem FirePS;
    public ParticleSystem PoisonPS;
    public ParticleSystem IcePS;
    public bool isFirePSActive = false;
    public bool isPoisonPSActive = false;
    public bool isIcePSActive = false;

    protected virtual void Init()
    {
        Player = LevelManager.Instance.player;
        baseFunctions = Player.GetComponent<BaseFunctions>();
        Manager = EnemyManager.Instance;
        Difficulty = Manager.Difficulty;
        SetStats(Manager.EnemyBaseHealth);
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        CheckEffectState();
        SetRotation();
        Move();
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
        if (finalDamage > 0)
            NumberManager.Instance.SpawnText(transform.position, finalDamage.ToString(), 1, Color.white);
        if (enemyEffects.isLifeSteal)
        {
            baseFunctions.RecoverHealth(finalDamage * 0.05f);
        }
        if (CurrentHealth <= 0)
        {
            if (enemyEffects.isExplode && !type.Equals(EnemyType.Exploder))
            {
                Explosion tempExplosion = Instantiate(enemyEffects.augmentPFList[0], transform.position, Quaternion.identity).GetComponent<Explosion>();
                tempExplosion.SetDamage(MaxHealth * 0.3f, true);
            }
            if (enemyEffects.isFreezeShard)
            {
                enemyEffects.ReleaseShard();
            }
            if (enemyEffects.isShieldSteal)
            {
                baseFunctions.DecreaseRecovery();
            }

            OnDeath();
        }
        else damageAudio.Play();
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
            case EnemyType.Sprinter:
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
            case EnemyType.Burster:
                Manager.PoolEnemy(gameObject, 8);
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
        Instantiate(deathAudio, transform.position, Quaternion.identity);
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

    public void SetParticleSystemState(int particleSystem, bool state)
    {
        if (particleSystem == 0)
        {
            if (state)
            {
                FirePS.Play();
                isFirePSActive = true;
            }
            if (!state)
            {
                FirePS.Stop();
                isFirePSActive = false;
            }
        }
        else if (particleSystem == 1)
        {
            if (state)
            {
                PoisonPS.Play();
                isPoisonPSActive = true;

            }
            if (!state)
            {
                PoisonPS.Stop();
                isPoisonPSActive = false;
            }
        }
        else if (particleSystem == 2)
        {
            if (state)
            {
                IcePS.Play();
                isIcePSActive = true;
            }
            if (!state)
            {
                IcePS.Stop();
                isIcePSActive = false;
            }
        }
    }

    public void CheckEffectState()
    {
        if (enemyEffects.fireTick > 0 && !isFirePSActive)
        {
            SetParticleSystemState(0, true);
        }
        if (enemyEffects.fireTick == 0 && isFirePSActive)
        {
            SetParticleSystemState(0, false);
        }
        if (enemyEffects.poisonTick > 0 && !isPoisonPSActive)
        {
            SetParticleSystemState(1, true);
        }
        if (enemyEffects.poisonTick == 0 && isPoisonPSActive)
        {
            SetParticleSystemState(1, false);
        }
        if (type != EnemyType.Boss)
        {
            if (enemyEffects.frozenAmount > 0 && !isIcePSActive)
            {
                SetParticleSystemState(2, true);
            }
            if (enemyEffects.frozenAmount == 0 && isIcePSActive)
            {
                SetParticleSystemState(2, false);
            }
        }
    }
}
