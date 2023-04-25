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
    private BuildManager buildManager;
    private AugmentManager augmentManager;
    public LevelCanvasManager levelCanvasManager;
    public SentryData[] possibleSentries;
    public EnemyManager enemyManager;
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
        gameManager = GameManager.Instance;
        augmentManager = AugmentManager.Instance;
        buildManager = new BuildManager();
    }

    public void Start()
    {
        levelCanvasManager = LevelCanvasManager.Instance;
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

        if (!AugmentManager.Instance.selectingAugment && Input.GetKeyDown(KeyCode.P)) {
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
        bool didSucceed = buildManager.PurchaseItemIfPossible(xenoriumCost, novaciteCost, voidStoneCost);
        levelCanvasManager.SetXenoriumAmount(buildManager.xenorium);
        levelCanvasManager.SetNovaciteAmount(buildManager.novacite);
        levelCanvasManager.SetVoidStoneAmount(buildManager.voidStone);
        return didSucceed;
    }

    public void SpawnAugmentChoice()
    {
        augmentManager.CreateAugmentChoices();
        if (augmentManager.augmentChoices.Count > 0)
        {
            levelCanvasManager.ShowAugmentChoices(augmentManager.augmentChoices);
        }
    }

    public ResourcesAmount GetResources()
    {
        return new ResourcesAmount(buildManager.xenorium, buildManager.novacite, buildManager.voidStone);
    }
}
