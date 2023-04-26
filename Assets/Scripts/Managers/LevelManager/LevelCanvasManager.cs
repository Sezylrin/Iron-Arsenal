using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LevelCanvasManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private FillBar healthBar;
    [SerializeField] private FillBar shieldBar;
    [SerializeField] private GameObject bossHealthObj;
    [SerializeField] private FillBar bossHealthBar;
    [SerializeField] private FillBar waveBar;

    [Header("Ore Labels")]
    [SerializeField] private GameObject resourceContainer;
    [SerializeField] private TMP_Text xenoriumLabel;
    [SerializeField] private TMP_Text novaciteLabel;
    [SerializeField] private TMP_Text voidStoneLabel;

    [Header("BuildMenu")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject sentryContainerPrefab;
    [SerializeField] private GameObject sentriesContent;
    [SerializeField] private LayerMask layer;

    [Header("Menus")]
    [SerializeField] private GameObject augmentMenu;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject shopMenu;
    private List<SentryBuildInitialise> allButtons = new List<SentryBuildInitialise>();

    public GameObject instantiatedToolTips;

    public bool overMenu = false;
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
        //LoadSentries();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || LevelManager.Instance.currentState == State.Normal)
        {
            CloseBuildMenu();
        }
        if (Input.GetMouseButtonDown(0) && LevelManager.Instance.currentState == State.Building)
        {
            Vector3 MousePos = MousePosition.MouseToWorld3D(Camera.main, -1);
            MousePos.y = transform.position.y;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer))
            {
                if (hit.collider.CompareTag("Socket"))
                {
                    SentrySocket socket = hit.collider.GetComponent<SentrySocket>();
                    if (!socket.HasSentry())
                    {
                        AssignSocket(socket);
                        OpenBuildMenu();
                    }
                }
            }
            else if (!overMenu)
            {
                AssignSocket(null);
                CloseBuildMenu();
            }
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

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
        overMenu = false;
        if (instantiatedToolTips)
            Destroy(instantiatedToolTips);
        instantiatedToolTips = null;
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    public void OpenShopMenu(ShopManager shopManager)
    {
        shopMenu.SetActive(true);
        resourceContainer.SetActive(false);
        shopMenu.GetComponent<ShopMenu>().OpenMenu(shopManager);
    }

    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        resourceContainer.SetActive(true);
    }

    public void AddSentryUI(SentryData sentryToAdd)
    {
        GameObject sentryContainer = Instantiate(sentryContainerPrefab);
        sentryContainer.transform.SetParent(sentriesContent.transform, false);
        SentryBuildInitialise initUI = sentryContainer.GetComponent<SentryBuildInitialise>();
        initUI.InitialiseSentryContainer(sentryToAdd);
        allButtons.Add(initUI);
    }

    public void AssignSocket(SentrySocket socket)
    {
        foreach (SentryBuildInitialise init in allButtons)
        {
            init.SetSocket(socket);
        }
    }

    public void SetHealth(int newHealth)
    {
        healthBar.SetSliderAmount(newHealth);
    }

    public void SetShield(int newShield)
    {
        shieldBar.SetSliderAmount(newShield);
    }

    public void SetWaveTimerBar(int newWaveTimer)
    {
        waveBar.SetSliderAmount(newWaveTimer);
    }

    public void EnableBossHealthBar()
    {
        bossHealthObj.SetActive(true);
        bossHealthBar.SetSliderAmount(100);
    }

    public void SetBossHealthBar(int newHealth)
    {
        bossHealthBar.SetSliderAmount(newHealth);
    }

    public void DisableBossHealthBar()
    {
        bossHealthObj.SetActive(false);
    }
}
