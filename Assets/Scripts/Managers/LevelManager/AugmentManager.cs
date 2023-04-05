using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

public class AugmentManager
{
    public List<AugmentData> allAugments;
    public List<AugmentData> augmentChoices;
    public bool selectingAugment { get; private set; } = false;

    public AugmentManager()
    {
        allAugments = new List<AugmentData>();
        string augmentPath = "Assets/ScriptableObjects/Augments/";
        foreach (AugmentName augmentName in Enum.GetValues(typeof(AugmentName)))
        {
            AugmentData augment = AssetDatabase.LoadAssetAtPath<AugmentData>(augmentPath + augmentName + ".asset");
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

    public void CreateAugmentChoices(int numAugments)
    {
        List<AugmentData> allAugmentsCopy = new(allAugments); // Create a copy of the original list
        List<AugmentData> randomAugments = new();
        int timesToLoop = numAugments;
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

    public void SelectAugment(AugmentData augmentSelected)
    {
        selectingAugment = false;
        allAugments.Remove(augmentSelected);
    }
}
