using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class BuildManager
{
    public List<SentryData> buildableSentries;

    //Resources
    public int iron { get; private set; }
    public int copper { get; private set; }
    public int gold { get; private set; }
    public BuildManager()
    {
        buildableSentries = new List<SentryData>();
        string sentryPath = "Assets/ScriptableObjects/Sentries/";

        foreach (SentryName sentryName in Enum.GetValues(typeof(SentryName)))
        {
            SentryData sentryData = AssetDatabase.LoadAssetAtPath<SentryData>(sentryPath + sentryName + ".asset");
            if (sentryData != null)
            {
                buildableSentries.Add(sentryData);
            }
            else
            {
                Debug.LogError("Unable to load SentryData asset: " + sentryName);
            }
        }
    }

    public bool CanBuildSentry(SentryName sentryName)
    {
        SentryData sentry = GetSentryData(sentryName);
        return iron >= sentry.ironCost && copper >= sentry.copperCost;
    }

    public void BuildSentry(SentryName sentryName)
    {
        SentryData sentry = GetSentryData(sentryName);
        iron -= sentry.ironCost;
        copper -= sentry.copperCost;
    }

    private SentryData GetSentryData(SentryName sentryName)
    {
        foreach (SentryData sentryData in buildableSentries)
        {
            if (sentryData.name == sentryName.ToString())
            {
                return sentryData;
            }
        }
        Debug.LogError("SentryData not found for SentryName: " + sentryName);
        return null;
    }

    public void GainIron(int ironToAdd)
    {
        iron += ironToAdd;
    }

    public void GainCopper(int copperToAdd)
    {
        copper += copperToAdd;
    }

    public void GainGold(int goldToAdd) {
        gold += goldToAdd;
    }
}
