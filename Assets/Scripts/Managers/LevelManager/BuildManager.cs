using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class BuildManager
{
    public List<SentryData> buildableSentries;

    //Resources
    public int xenorium { get; private set; }
    public int novacite { get; private set; }
    public int voidStone { get; private set; }
    public BuildManager()
    {
        buildableSentries = new List<SentryData>();
        string sentryPath = "Sentries/";

        foreach (SentryName sentryName in Enum.GetValues(typeof(SentryName)))
        {
            SentryData sentryData = Resources.Load<SentryData>(sentryPath + sentryName );
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
        return xenorium >= sentry.xenoriumCost && novacite >= sentry.novaciteCost && voidStone >= sentry.voidStoneCost;
    }

    public void BuildSentry(SentryName sentryName)
    {
        SentryData sentry = GetSentryData(sentryName);
        xenorium -= sentry.xenoriumCost;
        novacite -= sentry.novaciteCost;
        voidStone -= sentry.voidStoneCost;
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

    public void GainXenorium(int xenoriumToAdd)
    {
        xenorium += xenoriumToAdd;
    }

    public void GainNovacite(int novaciteToAdd)
    {
        novacite += novaciteToAdd;
    }

    public void GainVoidStone(int voidStoneToAdd) {
        voidStone += voidStoneToAdd;
    }

    public bool PurchaseItemIfPossible(int xenoriumCost, int novaciteCost, int voidStoneCost)
    {
        if (xenorium >= xenoriumCost && novacite >= novaciteCost && voidStone >= voidStoneCost)
        {
            xenorium -= xenoriumCost;
            novacite -= novaciteCost;
            voidStone -= voidStoneCost;
            return true;
        }
        return false;
    }
}
