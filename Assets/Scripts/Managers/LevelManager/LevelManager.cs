using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private GameManager gameManager;
    private BuildManager buildManager;
    private AugmentManager augmentManager;
    private LevelUIManager levelUIManager;
    public TurretBuildMenu BuildUi;
    public SentryData[] possibleSentries;


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
        BuildUi = GameObject.FindWithTag("UISelection").GetComponent<TurretBuildMenu>();
        gameManager = GameManager.Instance;
        buildManager = new BuildManager();
        augmentManager = new AugmentManager();
        levelUIManager = GetComponent<LevelUIManager>();
    }

    public void Start()
    {
        if (BuildUi)
            BuildUi.AddToMenu(possibleSentries[0]);
    }

    //Temp for now. Eventually these will be called by other classes
    public void Update()
    {
        //This will be done by mouseclick when we have menu functionality
        if (augmentManager.selectingAugment)
        {
            switch (Input.inputString)
            {
                case "1":
                    SelectAugment(0);
                    break;
                case "2":
                    if (augmentManager.augmentChoices.Count >= 2)
                    {
                        SelectAugment(1);
                    } 
                    break;
                case "3":
                    if (augmentManager.augmentChoices.Count >= 3)
                    {
                        SelectAugment(2);
                    }
                    break;
            }
        }

        if (!augmentManager.selectingAugment && Input.GetKeyDown(KeyCode.P)) {
            SpawnAugmentChoice();
        }
    }

    public bool CanBuildSentry(SentryName sentryName)
    {
        return buildManager.CanBuildSentry(sentryName);
    }

    public void BuildSentry(SentryName sentryName)
    {
        buildManager.BuildSentry(sentryName);
        levelUIManager.SetIronAmount(buildManager.iron);
        levelUIManager.SetCopperAmount(buildManager.copper);
    }

    public void GainIron(int ironToAdd)
    {
        buildManager.GainIron(ironToAdd);
        levelUIManager.SetIronAmount(buildManager.iron);
    }

    public void GainCopper(int copperToAdd)
    {
        buildManager.GainCopper(copperToAdd);
        levelUIManager.SetCopperAmount(buildManager.copper);
    }

    public void GainGold(int goldToAdd)
    {
        buildManager.GainGold(goldToAdd);
        levelUIManager.SetGoldAmount(buildManager.gold);
    }

    public void SpawnAugmentChoice(int numAugments = 3)
    {
        augmentManager.CreateAugmentChoices(numAugments);
        if (augmentManager.augmentChoices.Count > 0)
        {
            levelUIManager.ShowAugmentChoices(augmentManager.augmentChoices);
        }
    }

    public void SelectAugment(int augmentNumSelected)
    {
        AugmentData augmentSelected = augmentManager.augmentChoices[augmentNumSelected];
        //TODO: Apply this to turrets
        Debug.Log(augmentSelected.augName);
        augmentManager.SelectAugment(augmentSelected);
        levelUIManager.RemoveAugmentChoices();
    }
}
