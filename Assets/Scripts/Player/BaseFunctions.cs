using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFunctions : MonoBehaviour
{
    public bool debug = false;
    public float simulateDamage;
    public bool upgradeBase = false;
    public bool testingAdd = false;
    public bool removeTesting = false;

    public int currentLevel;

    public float baseHealth;
    public float baseHealthFactor;
    private float currentHealth;

    public float maxShieldHealth;
    public float maxShieldFactor;
    public float shieldRecoverRate;
    public float shieldRecoverFactor;
    public float shieldRecoverDelay;
    private float currentShield;

    public float collisionFactor = 0.3f;

    private float timeSinceDamage;

    public BaseEffects baseEffects;
    [System.Serializable]
    public class SocketSpawns
    {
        public Vector3[] pos;
    }
    public SocketSpawns[] relativeSpawnPos;
    public GameObject turretSocketPF;

    private int baseLevel = 0;

    private void Awake()
    {
        baseEffects = gameObject.AddComponent<BaseEffects>();
        baseEffects.baseFunction = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
        currentHealth = baseHealth;
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
            if (removeTesting)
            {
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
        if (currentShield - amount < 0f)
            currentShield = 0f;
        else
            currentShield -= amount;
        timeSinceDamage = shieldRecoverDelay;

    }

    private void RecoverShield(float amount)
    {
        if (currentShield + amount > maxShieldHealth)
            currentShield = maxShieldHealth;
        else
            currentShield += amount;
    }
    private void TakeHealthDamage(float amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
            if (GameManager.Instance)
            {
                //run function for losing
            }
        }
        else
            currentHealth -= amount;
    }

    public void RecoverHealth(float amount)
    {
        if (currentHealth + amount > baseHealth)
            currentHealth = baseHealth;
        else
            currentHealth += amount;
    }

    public void UpgradeBase()
    {
        foreach (Vector3 spawnPos in relativeSpawnPos[baseLevel].pos)
        {
            Instantiate(turretSocketPF, transform.position + spawnPos, Quaternion.identity, transform);
        }
        baseLevel++;
    }

    public void UpdateHealth()
    {
        baseHealth = StatsManager.Instance.healthFactor * baseHealthFactor;
        maxShieldHealth = StatsManager.Instance.healthFactor * maxShieldFactor;
        shieldRecoverRate = maxShieldHealth * shieldRecoverFactor;
    }

}
