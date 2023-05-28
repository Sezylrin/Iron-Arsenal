using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFunctions : MonoBehaviour
{
    public bool debug = false;
    public float simulateDamage;
    public bool upgradeBase = false;
    public bool testingAdd = false;

    public int currentLevel;

    public float maxHealth;
    public float maxHealthFactor;
    private float currentHealth;

    public float maxShieldHealth;
    public float maxShieldFactor;
    public float shieldRecoverRate;
    public float shieldRecoverFactor;
    public float shieldRecoverDelay;
    private float currentShield;

    public float collisionFactor = 0.3f;

    private float timeSinceDamage;

    private bool isRaged = false;
    private bool isRageTriggered = false;

    public GameObject ShieldWave;

    public Cannon cannon;
    public GameObject Arrow;
    public GameObject ArrowRotatePoint;

    public BaseEffects baseEffects;
    [System.Serializable]
    public class SocketSpawns
    {
        public Vector3[] pos;
    }
    public SocketSpawns[] relativeSpawnPos;
    public GameObject turretSocketPF;

    private int baseLevel = 0;

    public GameObject[] baseMesh;

    public BoxCollider boxCollider;

    private void Awake()
    {
        baseEffects = gameObject.AddComponent<BaseEffects>();
        baseEffects.baseFunction = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
        currentHealth = maxHealth;
        currentShield = maxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceDamage > 0)
            timeSinceDamage -= Time.deltaTime;
        else if (currentShield != maxShieldHealth)
            RecoverShield(shieldRecoverRate * Time.deltaTime);

        //debug functions
        if (debug)
        {
            //Debug.Log("Current Health: " + currentHealth + "\n" + "Current Shield: " + currentShield + "\n" + "Shield Regen Delay: " + timeSinceDamage);
            if (simulateDamage != 0)
            {
                TakeDamage(simulateDamage);
                simulateDamage = 0;
            }
            if (upgradeBase)
            {
                UpgradeBase();
                upgradeBase = false;
            }
            if (testingAdd)
            {
                testingAdd = false;
            }
        }
        if (isRaged && !isRageTriggered)
        {
            isRageTriggered = true;
            foreach (Sentry sentry in AugmentManager.Instance.activeSentries)
            {
                sentry.fireRate *= 1.2f;
            }
        }

        if (!isRaged && isRageTriggered)
        {
            isRageTriggered = false;
            foreach (Sentry sentry in AugmentManager.Instance.activeSentries)
            {
                sentry.fireRate /= 1.2f;
            }
        }


        if (EnemyManager.Instance.IsBossAlive)
        {
            if (Arrow.activeSelf == false)
            {
                Arrow.SetActive(true);
            }

            Vector3 bossPosition = EnemyManager.Instance.ActiveBoss.transform.position;
            bossPosition.y = ArrowRotatePoint.transform.position.y;
            ArrowRotatePoint.transform.LookAt(bossPosition);
        }

        if (!EnemyManager.Instance.IsBossAlive || Vector3.Distance(gameObject.transform.position, EnemyManager.Instance.ActiveBoss.transform.position) < 25)
        {
            if (Arrow.activeSelf == true)
            {
                Arrow.SetActive(false);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentShield > 0)
            TakeShieldDamage(amount);
        else
            TakeHealthDamage(amount);
    }

    private void TakeShieldDamage(float amount)
    {
        if (!baseEffects.isSegmentShield)
        {
            if (currentShield - amount < 0f)
            {
                currentShield = 0f;
                if (baseEffects.isShieldExplosion)
                    Instantiate(ShieldWave, transform.position, Quaternion.identity);
            }
            else
                currentShield -= amount;
        }
        else
        {
            float shieldSegment =  Mathf.CeilToInt(currentShield) % Mathf.CeilToInt((maxShieldHealth * 0.25f)) ;
            if (shieldSegment == 0)
                shieldSegment = Mathf.CeilToInt((maxShieldHealth * 0.25f));
            float overflow = shieldSegment - amount;
            if (overflow < 0)
            {
                currentShield -= (amount + overflow);
                if (baseEffects.isShieldExplosion)
                    Instantiate(ShieldWave, transform.position, Quaternion.identity);
            }
            else
                currentShield -= amount;
        }
        timeSinceDamage = shieldRecoverDelay;
        LevelCanvasManager.Instance.SetShield(ShieldPercentage());
    }

    private void RecoverShield(float amount)
    {
        if (currentShield + amount > maxShieldHealth)
            currentShield = maxShieldHealth;
        else
            currentShield += amount;
        LevelCanvasManager.Instance.SetShield(ShieldPercentage());
    }
    private void TakeHealthDamage(float amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
            if (GameManager.Instance)
            {
                GameManager.Instance.HandleDeath();
            }
        }
        else
            currentHealth -= amount;
        if (baseEffects.isRage && currentHealth <= maxHealth * 0.3)
        {
            isRaged = true;
        }
        LevelCanvasManager.Instance.SetHealth(HealthPercentage());
    }

    public void RecoverHealth(float amount)
    {
        if (currentHealth + amount > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
        if (baseEffects.isRage && currentHealth > maxHealth * 0.3)
        {
            isRaged = false;
        }
        LevelCanvasManager.Instance.SetHealth(HealthPercentage());
    }

    public void UpgradeBase()
    {
        if (baseLevel >= relativeSpawnPos.Length)
            return;
        if (baseLevel == 1)
        {
            baseMesh[0].SetActive(false);
            baseMesh[1].SetActive(true);
            transform.Translate(Vector3.up * 1.3f * transform.localScale.x);
            boxCollider.size *= 2.25f * transform.localScale.x;
            boxCollider.size = boxCollider.size + (Vector3.right * 2 * transform.localScale.x);
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y - (1.5f * transform.localScale.x), boxCollider.center.z);

        }
        foreach (Vector3 spawnPos in relativeSpawnPos[baseLevel].pos)
        {
            GameObject socket = Instantiate(turretSocketPF, transform.position + (spawnPos * transform.localScale.x), Quaternion.identity, transform);
            socket.transform.RotateAround(transform.position, Vector3.up, Mathf.Ceil(transform.eulerAngles.y));
        }
        baseLevel++;
    }

    public void UpdateHealth()
    {
        float newHealth = StatsManager.Instance.healthFactor * maxHealthFactor;
        float healthDifference = newHealth - maxHealth;
        maxHealth = StatsManager.Instance.healthFactor * maxHealthFactor;
        RecoverHealth(healthDifference);
        maxShieldHealth = StatsManager.Instance.healthFactor * maxShieldFactor;
        shieldRecoverRate = maxShieldHealth * shieldRecoverFactor;
    }

    public int HealthPercentage()
    {
        return Mathf.CeilToInt((currentHealth / maxHealth) * 100);
    }

    public int ShieldPercentage()
    {
        return Mathf.CeilToInt((currentShield / maxShieldHealth) * 100);
    }
    public void DecreaseRecovery()
    {
        if (timeSinceDamage > 0)
        {
            timeSinceDamage -= 0.25f;
        }
    }

    public void ShopRecoverHealth()
    {
        RecoverHealth(maxHealth * 0.2f);
    }
}
