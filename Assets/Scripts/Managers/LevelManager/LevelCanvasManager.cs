using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

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

    [Header("Timer")]
    [SerializeField] private GameObject timer;
    [SerializeField] private TMP_Text timerTxt;

    [Header("Menus")]
    [SerializeField] private GameObject augmentMenu;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject attributeMenu;
    private List<SentryBuildInitialise> allButtons = new List<SentryBuildInitialise>();

    public List<SentrySocket> allSockets = new List<SentrySocket>();

    public Material[] mats;

    private bool changedMat = false;

    public GameObject instantiatedToolTip;

    public bool overMenu = false;

    public GameObject mapUI;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || LevelManager.Instance.currentState == State.Normal)
        {
            CloseBuildMenu();
        }
        if(LevelManager.Instance.currentState == State.Building && !changedMat)
        {
            changedMat = true;
            foreach(SentrySocket socket in allSockets)
            {
                if (!socket.HasSentry())
                {
                    socket.meshRender.material = mats[0];
                }
            }
        }
        else if (LevelManager.Instance.currentState != State.Building && changedMat)
        {
            changedMat = false;
            foreach (SentrySocket socket in allSockets)
            {
                socket.meshRender.material = socket.mat;
            }
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
                        foreach (SentrySocket sockets in allSockets)
                        {
                            if (!sockets.HasSentry())
                            {
                                sockets.meshRender.material = mats[0];
                            }
                        }
                        socket.meshRender.material = mats[1];
                    }
                }
            }
            else if (!overMenu)
            {
                foreach (SentrySocket socket in allSockets)
                {
                    if (!socket.HasSentry())
                    {
                        socket.meshRender.material = mats[0];
                    }
                }
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

    public void ShowAttributeChoices()
    {
        attributeMenu.GetComponent<AttributeMenu>().SetAttributes();
        attributeMenu.SetActive(true);
    }

    public void RemoveAugmentChoices()
    {
        augmentMenu.SetActive(false);
    }

    public void RemoveAttributeChoices()
    {
        attributeMenu.SetActive(false);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
        overMenu = false;
        if (instantiatedToolTip)
            Destroy(instantiatedToolTip);
        instantiatedToolTip = null;
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

    public void SetHealth(float newHealth)
    {
        healthBar.SetSliderAmount(newHealth);
    }

    public void SetShield(float newShield)
    {
        shieldBar.SetSliderAmount(newShield);
    }

    public void SetWaveTimerBar(float newWaveTimer)
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

    public void ToggleMap(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!mapUI.activeSelf && GameManager.Instance.currentSelection == CurrentSelection.Playing)
            {
                mapUI.SetActive(true);
            }
            else
            {
                CloseMap();
            }
        }
    }

    public void CloseMap()
    {
        mapUI.SetActive(false);
    }

    public void StartTimer(int seconds)
    {
        timer.SetActive(true);
        StartCoroutine(CountdownCoroutine(seconds));
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        while (seconds > 0)
        {
            UpdateTimer(seconds);
            yield return new WaitForSeconds(1);
            seconds--;
        }

        UpdateTimer(0);
        timer.SetActive(false);
    }

    private void UpdateTimer(int secondsRemaining)
    {
        timerTxt.text = secondsRemaining.ToString();
    }

}