using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Augments : int
{
    ExplosiveProjectile,
    None
}
public class AugmentManager : MonoBehaviour
{
    public static AugmentManager Instance { get; private set; }
    public bool debug;
    public Augments debugAugment;
    public bool addAugmentToActive = false;

    public List<Augments> activeAugments = new List<Augments>();
    public List<Sentry> activeSentries = new List<Sentry>();
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
        if (!augmentToAdd.Equals(Augments.None))
            activeAugments.Add(augmentToAdd);
        foreach (Sentry sentry in activeSentries)
            sentry.AddAugmentToList(augmentToAdd);
    }

    public void AddAugmentToProjectile(Augments AugmentToInstall, GameObject Target, Projectile projData = null)
    {
        switch ((int)AugmentToInstall)
        {
            case 0:
                Target.AddComponent<AExplosvie>().baseProjectile = projData;
                break;
            default:
                Debug.Log("Wrong Value");
                Debug.Break();
                break;


        }
            
    }
}
