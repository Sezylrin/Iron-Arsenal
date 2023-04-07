using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFunctions : MonoBehaviour
{
    public bool debug = false;
    public float simulateDamage;

    public float baseHealth;
    private float currentHealth;

    public float maxShieldHealth;
    public float shieldRecoverRate;
    public float shieldRecoverDelay;
    private float currentShield;

    private float timeSinceDamage;
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("Current Health: " + currentHealth + "\n" + "Current Shield: " + currentShield + "\n" + "Shield Regen Delay: " + timeSinceDamage);
            if (simulateDamage != 0)
            {
                TakeDamage(simulateDamage);
                simulateDamage = 0;
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
}
