using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public MapGenerator mapGenerator;
    [Header("Sequence 1")]
    [Tooltip("Dialogue that plays at the start of tutorial")]
    public GameObject welcomeDialogue;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    [Tooltip("Dialogue that plays when entering the 1st zone")]
    public GameObject enemiesDialogue;
    [Tooltip("Dialogue that plays when killing any enemy")]
    public GameObject resourcesDialogue;

    [Header("Sequence 2")]
    public GameObject endCombatDialogue;
    public GameObject sentryDialogue;

    [Header("Sequence 3")]
    public GameObject mapDialogue;
    public GameObject map;
    public GameObject radarDialogue;
    [Header("Sequence 4")]
    public GameObject eventsContainer;
    public GameObject outpostDialogue;
    [Header("Sequence 5")]
    public GameObject shop;
    public GameObject shopMenu;
    public GameObject statsTab;
    public GameObject augmentsTab;
    public GameObject sentriesTab;
    public GameObject spendDialogue;
    public GameObject upgradesDialogue;
    public GameObject statsDialogue;
    public GameObject augmentsDialogue;
    public GameObject shopSentriesDialogue;
    [Header("Sequence 6")]
    public GameObject events;
    public GameObject nextZoneDialogue;
    public GameObject eventsDialogue;
    public GameObject miningDialogue;
    public GameObject rocketsDialogue;
    public GameObject chestDialogue;
    public GameObject ruinsDialogue;
    public GameObject bossDialogue;

    private bool miningEventViewed;
    private bool rocketsEventViewed;
    private bool chestEventViewed;
    private bool ruinsEventViewed;
    private bool bossEventViewed;
    [Header("Sequence 7")]
    public GameObject exitDialogue;

    public static TutorialManager Instance {get; private set;}

    void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        StartSequence1();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DebugHere() => Debug.Log("Debug here");

    private void StartSequence1()
    {
        welcomeDialogue.SetActive(true);
    }

    public void Collision1()
    {
        enemy1.SetActive(true);
        enemy2.SetActive(true);
        enemy3.SetActive(true);
        enemiesDialogue.SetActive(true);
        StartCoroutine(Collision1Coroutine());
    }

    IEnumerator Collision1Coroutine()
    {
        while (enemy1.activeSelf && enemy2.activeSelf && enemy3.activeSelf) yield return null;

        resourcesDialogue.SetActive(true);

        while (enemy1.activeSelf || enemy2.activeSelf || enemy3.activeSelf) yield return null;

        resourcesDialogue.SetActive(false);

        StartCoroutine(Sequence2Coroutine());
    }

    IEnumerator Sequence2Coroutine()
    {
        endCombatDialogue.SetActive(true);

        while (LevelManager.Instance.currentState != State.Building) yield return null;

        endCombatDialogue.SetActive(false);
        LevelManager.Instance.GainXenorium(100);
        sentryDialogue.SetActive(true);

        while (LevelManager.Instance.currentState != State.Normal) yield return null;

        sentryDialogue.SetActive(false);
        StartCoroutine(Sequence3Coroutine());
    }

    IEnumerator Sequence3Coroutine()
    {
        mapDialogue.SetActive(true);

        while (!map.activeSelf)
        {
            yield return null;
        }

        mapDialogue.SetActive(false);
        radarDialogue.SetActive(true);
    }

    public void Collision4()
    {
        radarDialogue.SetActive(false);
        outpostDialogue.SetActive(true);
        StartCoroutine(Sequence4Coroutine());
    }

    IEnumerator Sequence4Coroutine()
    {
        while (eventsContainer.transform.childCount != 0)
        {
            yield return null;
        }

        outpostDialogue.SetActive(false);
        StartCoroutine(Sequence5Coroutine());
    }

    IEnumerator Sequence5Coroutine()
    {
        shop.SetActive(true);
        spendDialogue.SetActive(true);

        while (!shopMenu.activeSelf)
        {
            yield return null;
        }

        spendDialogue.SetActive(false);
        upgradesDialogue.SetActive(true);

        while (statsTab.GetComponent<Image>().sprite != statsTab.GetComponent<Tab>().activeImg)
        {
            yield return null;
        }

        upgradesDialogue.SetActive(false);
        statsDialogue.SetActive(true);

        while (augmentsTab.GetComponent<Image>().sprite != augmentsTab.GetComponent<Tab>().activeImg)
        {
            yield return null;
        }

        statsDialogue.SetActive(false);
        augmentsDialogue.SetActive(true);

        while (sentriesTab.GetComponent<Image>().sprite != sentriesTab.GetComponent<Tab>().activeImg)
        {
            yield return null;
        }

        augmentsDialogue.SetActive(false);
        shopSentriesDialogue.SetActive(true);

        while (shopMenu.activeSelf)
        {
            yield return null;
        }

        shopSentriesDialogue.SetActive(false);
        nextZoneDialogue.SetActive(true);
    }

    public void Collision6()
    {
        nextZoneDialogue.SetActive(false);
        StartCoroutine(Sequence6Coroutine());
    }

    IEnumerator Sequence6Coroutine()
    {
        events.SetActive(true);
        eventsDialogue.SetActive(true);
        while (!miningEventViewed)
        yield return null;
    }

    IEnumerator Sequence7Coroutine()
    {
        exitDialogue.SetActive(true);
        yield return null;
    }

    public void MiningTrigger()
    {
        miningEventViewed = true;
        miningDialogue.SetActive(true);
    }

    public void RocketsTrigger()
    {
        rocketsEventViewed = true;
        rocketsDialogue.SetActive(true);
    }

    public void ChestTrigger()
    {
        chestEventViewed = true;
        chestDialogue.SetActive(true);
    }

    public void RuinsTrigger()
    {
        ruinsEventViewed = true;
        ruinsDialogue.SetActive(true);
    }

    public void BossTrigger()
    {
        bossEventViewed = true;
        bossDialogue.SetActive(true);
    }

}