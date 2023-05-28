using UnityEngine;
using System.Collections.Generic;
public enum State
{
    Normal,
    Building
}

public struct ResourcesAmount
{
    public ResourcesAmount(int xenorium, int novacite, int voidStone)
    {
        this.xenorium = xenorium;
        this.novacite = novacite;
        this.voidStone = voidStone;
    }

    public int xenorium { get; }
    public int novacite { get; }
    public int voidStone { get; }

}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private GameManager gameManager;
    public BuildManager buildManager;
    private AugmentManager augmentManager;
    public LevelCanvasManager levelCanvasManager;
    public SentryData[] possibleSentries;
    public EnemyManager enemyManager;
    public GameObject player;
    public BaseFunctions playerFunctions;
    public Mining playerMining;
    public State currentState = State.Normal;

    [Header("Audio")]
    [SerializeField] private AudioClip gameSound;
    [SerializeField] private AudioClip pauseSound;

    [Header("Debug")]
    [SerializeField] private bool unlimitedResources = false;

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
        player = GameObject.FindWithTag("Player");
    }

    public void Start()
    {
        augmentManager = AugmentManager.Instance;
        buildManager = new BuildManager();
        gameManager = GameManager.Instance;
        levelCanvasManager = LevelCanvasManager.Instance;
        playerFunctions = player.GetComponent<BaseFunctions>();
        playerMining = player.GetComponent<Mining>();
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = gameSound;
        audioSource.Play();
    }

    //Temp for now. Eventually these will be called by other classes
    public void Update()
    {
        if (currentState == State.Normal && Input.GetKeyDown(KeyCode.B))
        {
            currentState = State.Building;
            GameManager.Instance.PauseGame();
            
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            currentState = State.Normal;
            GameManager.Instance.ResumeGame();
            LevelCanvasManager.Instance.CloseBuildMenu();
            LevelCanvasManager.Instance.CloseRemoveSentryBtn();
        }
    }
    public bool CanBuildSentry(SentryName sentryName)
    {
        if (unlimitedResources)
        {
            return true;
        }
        return buildManager.CanBuildSentry(sentryName);
    }

    public void BuildSentry(SentryName sentryName)
    {
        buildManager.BuildSentry(sentryName);
        levelCanvasManager.SetXenoriumAmount(buildManager.xenorium);
        levelCanvasManager.SetNovaciteAmount(buildManager.novacite);
        levelCanvasManager.SetVoidStoneAmount(buildManager.voidStone);
    }

    public void GainXenorium(int xenoriumToAdd)
    {
        buildManager.GainXenorium(xenoriumToAdd);
        levelCanvasManager.SetXenoriumAmount(buildManager.xenorium);
    }

    public int GetXenorium()
    {
        return buildManager.xenorium;
    }

    public int GetNovacite()
    {
        return buildManager.novacite;
    }

    public int GetVoidStone()
    {
        return buildManager.voidStone;
    }

    public void GainNovacite(int novaciteToAdd)
    {
        buildManager.GainNovacite(novaciteToAdd);
        levelCanvasManager.SetNovaciteAmount(buildManager.novacite);
    }

    public void GainVoidStone(int voidStoneToAdd)
    {
        buildManager.GainVoidStone(voidStoneToAdd);
        levelCanvasManager.SetVoidStoneAmount(buildManager.voidStone);
    }

    public bool PurchaseItemIfPossible(int xenoriumCost, int novaciteCost, int voidStoneCost)
    {
        if (unlimitedResources)
        {
            return true;
        }

        bool didSucceed = buildManager.PurchaseItemIfPossible(xenoriumCost, novaciteCost, voidStoneCost);
        levelCanvasManager.SetXenoriumAmount(buildManager.xenorium);
        levelCanvasManager.SetNovaciteAmount(buildManager.novacite);
        levelCanvasManager.SetVoidStoneAmount(buildManager.voidStone);
        return didSucceed;
    }

    public bool CanPurchaseItem(int xenoriumCost, int novaciteCost, int voidStoneCost)
    {
        return buildManager.CanPurchaseItem(xenoriumCost, novaciteCost, voidStoneCost);
    }

    public void SpawnAugmentChoice()
    {
        augmentManager.CreateAugmentChoices();
        if (augmentManager.augmentChoices.Count > 0)
        {
            levelCanvasManager.ShowAugmentChoices(augmentManager.augmentChoices);
        }
    }

    public void SpawnAttributeChoice()
    {
        LevelCanvasManager.Instance.ShowAttributeChoices();
    }

    public ResourcesAmount GetResources()
    {
        return new ResourcesAmount(buildManager.xenorium, buildManager.novacite, buildManager.voidStone);
    }

    public void PlayGameSound()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = gameSound;
        audioSource.Play();
    }
    
    public void PlayPauseSound()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = pauseSound;
        audioSource.Play();
    }
}