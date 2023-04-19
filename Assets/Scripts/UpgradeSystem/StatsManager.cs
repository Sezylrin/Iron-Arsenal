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
    public void UpgradeDamage()
    {
        physicalDamage *= 1.1f;
    }
    public void UpgradeDamage(float damage)
    {
        physicalDamage += damage;
    }

    public void UpgradeElemental()
    {
        elementalDamage *= 1.1f;
    }

    public void UpgradeElemental(float damage)
    {
        elementalDamage += damage;
    }

    public void UpgradeHealth()
    {
        healthFactor *= 1.1f;
    }

    public void UpgradeHealth(float health)
    {
        healthFactor += health;
    }
}
