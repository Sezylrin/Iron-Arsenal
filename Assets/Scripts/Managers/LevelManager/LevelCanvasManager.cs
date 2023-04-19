using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LevelCanvasManager : MonoBehaviour
{
    [Header("Ore Labels")]
    [SerializeField] private TMP_Text xenoriumLabel;
    [SerializeField] private TMP_Text novaciteLabel;
    [SerializeField] private TMP_Text voidStoneLabel;

    [Header("Augments")]
    [SerializeField] private TMP_Text firstAugName;
    [SerializeField] private TMP_Text firstAugDesc;
    [SerializeField] private TMP_Text secondAugName;
    [SerializeField] private TMP_Text secondAugDesc;
    [SerializeField] private TMP_Text thirdAugName;
    [SerializeField] private TMP_Text thirdAugDesc;

    [Header("BuildMenu")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject sentryContainerPrefab;
    [SerializeField] private GameObject sentriesContent;

    [Header("Containers")]
    [SerializeField] private GameObject augmentContainer;
    [SerializeField] private GameObject buildMenu;

    public static LevelCanvasManager Instance { get; private set; }

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

    private void Start()
    {
        closeBtn.onClick.AddListener(CloseBuildMenu);
        LoadSentries();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBuildMenu();
        }
    }

    public void SetXenoriumAmount(int xenoriumAmount)
    {
        xenoriumLabel.text = xenoriumAmount.ToString();
    }

    public void SetNovaciteAmount(int novaciteAmount)
    {
        novaciteLabel.text = novaciteAmount.ToString();
    }

    public void SetVoidStoneAmount(int voidStoneAmount)
    {
        voidStoneLabel.text = voidStoneAmount.ToString();
    }

    public void ShowAugmentChoices(List<AugmentData> augments)
    {
        firstAugName.text = augments[0].augName + ": Press 1";
        firstAugDesc.text = augments[0].description;
        if (augments.Count >= 2)
        {
            secondAugName.text = augments[1].augName + ": Press 2";
            secondAugDesc.text = augments[1].description;
        }
        else
        {
            secondAugName.text = "Empty Augment";
            secondAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        if (augments.Count >= 3)
        {
            thirdAugName.text = augments[2].augName + ": Press 3";
            thirdAugDesc.text = augments[2].description;
        }
        else
        {
            thirdAugName.text = "Empty Augment";
            thirdAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        augmentContainer.SetActive(true);
    }

    public void RemoveAugmentChoices()
    {
        augmentContainer.SetActive(false);
    }

    private void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    private void LoadSentries()
    {
        List<SentryData> sentries = new();
        string sentryPath = "Sentries/";

        foreach(SentryName sentryName in Enum.GetValues(typeof(SentryName)))
        {
            SentryData sentryData = Resources.Load<SentryData>(sentryPath + sentryName);
            if (sentryData != null)
            {
                sentries.Add(sentryData);
                GameObject sentryContainer = Instantiate(sentryContainerPrefab);
                sentryContainer.transform.SetParent(sentriesContent.transform, false);
                sentryContainer.GetComponent<SentryBuildInitialise>().InitialiseSentryContainer(sentryData);
            }
            else 
            {
                Debug.LogError("Unable to load SentryData asset: " + sentryName);
            }
        }
    }
}
