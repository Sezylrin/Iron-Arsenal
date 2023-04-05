using UnityEngine;
using System.Collections.Generic;
public enum State
{
    Normal,
    Building
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private GameManager gameManager;
    private BuildManager buildManager;
    private AugmentManager augmentManager;
    public LevelCanvasManager levelCanvasManager;
    public TurretBuildMenu BuildUi;
    public SentryData[] possibleSentries;
    public TempEnemyManager EnemyManager;
    public State currentState = State.Normal;

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
    }

    public void Start()
    {
        levelCanvasManager = LevelCanvasManager.Instance;
        if (BuildUi)
            BuildUi.AddToMenu(possibleSentries[0]);
    }

    //Temp for now. Eventually these will be called by other classes
    public void Update()
    {
        if (currentState == State.Normal && Input.GetKeyDown(KeyCode.B))
        {
            currentState = State.Building;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            currentState = State.Normal;
        }
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
        levelCanvasManager.SetIronAmount(buildManager.iron);
        levelCanvasManager.SetCopperAmount(buildManager.copper);
        levelCanvasManager.SetGoldAmount(buildManager.gold);
    }

    public void GainIron(int ironToAdd)
    {
        buildManager.GainIron(ironToAdd);
        levelCanvasManager.SetIronAmount(buildManager.iron);
    }

    public void GainCopper(int copperToAdd)
    {
        buildManager.GainCopper(copperToAdd);
        levelCanvasManager.SetCopperAmount(buildManager.copper);
    }

    public void GainGold(int goldToAdd)
    {
        buildManager.GainGold(goldToAdd);
        levelCanvasManager.SetGoldAmount(buildManager.gold);
    }

    public void SpawnAugmentChoice(int numAugments = 3)
    {
        augmentManager.CreateAugmentChoices(numAugments);
        if (augmentManager.augmentChoices.Count > 0)
        {
            levelCanvasManager.ShowAugmentChoices(augmentManager.augmentChoices);
        }
    }

    public void SelectAugment(int augmentNumSelected)
    {
        AugmentData augmentSelected = augmentManager.augmentChoices[augmentNumSelected];
        //TODO: Apply this to turrets
        Debug.Log(augmentSelected.augName);
        augmentManager.SelectAugment(augmentSelected);
        levelCanvasManager.RemoveAugmentChoices();
    }

    public int GetMode()
    {
        if (augmentManager.currentAugment)
            return augmentManager.currentAugment.mode;
        else
            return 0;
    }
}
