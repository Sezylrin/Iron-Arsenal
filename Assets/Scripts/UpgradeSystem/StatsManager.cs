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

    private float physicalScale = 0;
    private float elementalScale = 0;
    private float healthScale = 0;

    private int physicalLevel = 0;
    private int elementalLevel = 0;
    private int healthLevel = 0;

    public float initialBoost;
    public float diminishingValue;

    public bool debug = false;
    public bool upgradeDamage = false;
    public bool upgradeElemental = false;
    public bool upgradeHealth = false;

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
        physicalDamage = basePhysical;
        elementalDamage = baseElemental;
        healthFactor = baseHealth;
    }

    private void Start()
    {
        playerFunctions = GameObject.FindWithTag("Player").GetComponent<BaseFunctions>();
        
    }

    private void Update()
    {
        if (debug)
        {
            if (upgradeDamage)
            {
                upgradeDamage = !upgradeDamage;
                UpgradeDamage();
            }
            if (upgradeElemental)
            {
                upgradeElemental = !upgradeElemental;
                UpgradeElemental();
            }
            if (upgradeHealth)
            {
                upgradeHealth = !upgradeHealth;
                UpgradeHealth();
            }
        }
    }
    public void UpgradeDamage()
    {
        physicalScale += PhysicalUpgradeAmount();
        physicalDamage = basePhysical * (1f + physicalScale);
        physicalLevel++;
    }

    public float PhysicalUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * physicalLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

    public void UpgradeElemental()
    {
        elementalScale += ElementalUpgradeAmount();
        elementalDamage = baseElemental * (1f + elementalScale);
        elementalLevel++;
    }
    public float ElementalUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * elementalLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

    public void UpgradeHealth()
    {
        healthScale += HealthUpgradeAmount();
        healthFactor = baseHealth * (1f + healthScale);
        healthLevel++;
        playerFunctions.UpdateHealth();
    }
    public float HealthUpgradeAmount()
    {
        float dimishingAmount = diminishingValue * healthLevel;
        return (initialBoost - dimishingAmount) <= (initialBoost * 0.1) ? initialBoost * 0.1f : initialBoost - dimishingAmount;
    }

}
