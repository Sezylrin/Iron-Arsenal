using System.Collections.Generic;
using UnityEngine;
using System;

public enum Augments : int
{
    ExplosiveProjectile,
    Pierce,
    ExplosiveSize,
    KnockBack,
    ExplosiveEnemy,
    FireProjectile,
    FireTrail,
    FireSpread,
    DecreaseResistence,
    PoisonProjectile,
    PoisonMaxHealth,
    InstantDeath,
    FreezeProjectile,
    FreezeDeath,
    FreezeShards,
    RangeDamage,
    DoubleProjectiles,
    FireRate,
    RangeUp,
    BossDamageIncrease,
    Thorns,
    FasterShieldRegen,
    MoreShield,
    ShieldSteal,
    LifeSteal,
    Rage,
    SegmentShield,
    ShieldExplosion,
    None
}
public class AugmentManager : MonoBehaviour
{
    public static AugmentManager Instance { get; private set; }
    public bool debug;
    public Augments debugAugment;
    public bool addAugmentToActive = false;
    public float baseDamage;

    public List<Augments> activeAugments = new List<Augments>();
    public List<Sentry> activeSentries = new List<Sentry>();
    public List<GameObject> augmentPrefabs = new List<GameObject>();
    private BaseFunctions playerFunctions;

    public List<AugmentData> allAugments;
    public List<AugmentData> augmentChoices;
    public bool selectingAugment { get; private set; } = false;

    // Start is called before the first frame update
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

        allAugments = new List<AugmentData>();
        string augmentPath = "Augments/";
        foreach (AugmentName augmentName in Enum.GetValues(typeof(AugmentName)))
        {
            AugmentData augment = Resources.Load<AugmentData>(augmentPath + augmentName);
            if (augment != null)
            {
                allAugments.Add(augment);
            }
            else
            {
                Debug.LogError("Unable to load SentryData asset: " + augmentName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (addAugmentToActive)
            {
                AddAugment(debugAugment);
                addAugmentToActive = false;
            }
        }
    }
    public void AddAugment(Augments augmentToAdd)
    {
        selectingAugment = false;
        if (!augmentToAdd.Equals(Augments.None) && !activeAugments.Contains(augmentToAdd))
            activeAugments.Add(augmentToAdd);
        foreach (Sentry sentry in activeSentries)
        {
            sentry.AddAugmentToList(augmentToAdd);
        }

        for (int i = allAugments.Count - 1; i >= 0; i--)
        {
            if (allAugments[i].augmentType == augmentToAdd)
            {
                allAugments.RemoveAt(i);
            }
        }

        playerFunctions.baseEffects.UpdateAugments();
    }

    public AugmentBase AddAugmentToProjectile(Augments AugmentToInstall, GameObject Target, Projectile projData = null)
    {
        switch ((int)AugmentToInstall)
        {
            case (int)Augments.ExplosiveProjectile:
                AExplosive tempExplosive = Target.AddComponent<AExplosive>();
                tempExplosive.baseProjectile = projData;
                tempExplosive.explosionPF = augmentPrefabs[0];
                return tempExplosive;
            case (int)Augments.Pierce:
                APierce tempPierce = Target.AddComponent<APierce>();
                tempPierce.baseProjectile = projData;
                return tempPierce;
            case (int)Augments.ExplosiveSize:
                AExplosionScale tempScale = Target.AddComponent<AExplosionScale>();
                tempScale.baseProjectile = projData;
                return tempScale;
            case (int)Augments.KnockBack:
                AKnockBack tempKB = Target.AddComponent<AKnockBack>();
                tempKB.baseProjectile = projData;
                return tempKB;
            case (int)Augments.FireProjectile:
                AFireProjectiles tempFP = Target.AddComponent<AFireProjectiles>();
                tempFP.baseProjectile = projData;
                return tempFP;
            case (int)Augments.PoisonProjectile:
                APoison tempP = Target.AddComponent<APoison>();
                tempP.baseProjectile = projData;
                return tempP;
            case (int)Augments.FreezeProjectile:
                AFreezeProjectile tempFreeze = Target.AddComponent<AFreezeProjectile>();
                tempFreeze.baseProjectile = projData;
                return tempFreeze;
            case (int)Augments.RangeDamage:
                ARangeDamage tempRD = Target.AddComponent<ARangeDamage>();
                tempRD.baseProjectile = projData;
                return tempRD;
            default:
                return null;
        }  
    }

    public void CreateAugmentChoices()
    {
        List<AugmentData> allAugmentsCopy = new(allAugments); // Create a copy of the original list
        List<AugmentData> randomAugments = new();
        int timesToLoop = 3;
        int index;
        System.Random random = new();

        if (timesToLoop > allAugments.Count)
        {
            timesToLoop = allAugments.Count;
        }

        for (int i = 0; i < timesToLoop; i++)
        {
            // Generate a random index
            index = random.Next(allAugmentsCopy.Count);

            // Add the augment to the list and remove it from the allAugmentsCopy list to avoid duplicates
            randomAugments.Add(allAugmentsCopy[index]);
            allAugmentsCopy.RemoveAt(index);
        }

        selectingAugment = true;
        augmentChoices = randomAugments;
    }
}
