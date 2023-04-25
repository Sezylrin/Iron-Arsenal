using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SentryManager : MonoBehaviour
{
    public static SentryManager Instance { get; private set; }

    public List<SentryData> LockedSentries = new List<SentryData>();

    private void Awake()
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

    void Start()
    {
        string sentryPath = "Sentries/";
        foreach (SentryName sentryName in Enum.GetValues(typeof(SentryName)))
        {

            SentryData sentryData = Resources.Load<SentryData>(sentryPath + sentryName);

            if (sentryData != null)
            {
                if (!sentryData.Sentry.Equals(SentryName.BasicSentry))
                    LockedSentries.Add(sentryData);
                else
                    AddSentry(sentryData);
            }
        }
    }

    public void AddSentry(SentryData sentryName)
    {
        SentryData data = null;
        foreach (SentryData locked in LockedSentries)
        {
            if (locked.Sentry.Equals(sentryName.Sentry))
            {
                data = locked;
                break;
            }
        }
        if (data)
            LevelCanvasManager.Instance.AddSentryUI(data);
        else
            LevelCanvasManager.Instance.AddSentryUI(sentryName);
        LockedSentries.Remove(data);
    }
}
