using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatAttribute : int
{
    Physical,
    Elemental,
    Health,
    Undefined
}
public class StatsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static StatsManager Instance { get; private set; }
    public float physicalDamage;
    public float elementalDamage;
    public float healthFactor;

    public float basePhysical;
    public float baseElemental;
    public float baseHealth;

    private int physicalLevel = 0;
    private int elementalLevel = 0;
    private int healthLevel = 0;

    public float initialBoost;
    public float diminishingValue;

    private BaseFunctions playerFunctions;
    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerFunctions = GameObject.FindWithTag("Player").GetComponent<BaseFunctions>();
    }
    public void UpgradeDamage()
    {
        float dimishingAmount = diminishingValue * physicalLevel;
        physicalDamage = basePhysical * (1f + initialBoost - (initialBoost - dimishingAmount) <= (initialBoost * 0.1f)? initialBoost * 0.9f : dimishingAmount);
        physicalLevel++;
    }

    public float PhysicalUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * physicalLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

    public void UpgradeElemental()
    {
        float dimishingAmount = diminishingValue * elementalLevel;
        elementalDamage *= baseElemental * (1f + initialBoost - (initialBoost - dimishingAmount) <= (initialBoost * 0.1f) ? initialBoost * 0.9f : dimishingAmount);
        elementalLevel++;
    }
    public float ElementalUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * elementalLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

    public void UpgradeHealth()
    {
        float dimishingAmount = diminishingValue * healthLevel;
        healthFactor *= baseHealth * (1f + initialBoost - (initialBoost - dimishingAmount) <= (initialBoost * 0.1f) ? initialBoost * 0.9f : dimishingAmount);
        healthLevel++;
        playerFunctions.UpdateHealth();
    }
    public float HealthUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * healthLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

}
