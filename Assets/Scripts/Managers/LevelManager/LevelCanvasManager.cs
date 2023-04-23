using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LevelCanvasManager : MonoBehaviour
{
    [Header("Ore Labels")]
    [SerializeField] private GameObject resourceContainer;
    [SerializeField] private TMP_Text xenoriumLabel;
    [SerializeField] private TMP_Text novaciteLabel;
    [SerializeField] private TMP_Text voidStoneLabel;

    [Header("BuildMenu")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject sentryContainerPrefab;
    [SerializeField] private GameObject sentriesContent;

    [Header("Menus")]
    [SerializeField] private GameObject augmentMenu;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject shopMenu;

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
        augmentMenu.GetComponent<AugmentMenu>().CreateAugmentChoices(augments);
        augmentMenu.SetActive(true);
    }

    public void RemoveAugmentChoices()
    {
        augmentMenu.SetActive(false);
    }

    private void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        resourceContainer.SetActive(false);
    }

    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        resourceContainer.SetActive(true);
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
