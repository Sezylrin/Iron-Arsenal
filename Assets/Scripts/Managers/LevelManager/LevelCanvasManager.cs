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

    [Header("Remove Sentry Button")]
    [SerializeField] private GameObject removeSentryBtn;

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

    private SentrySocket socket;

    // Radar stuff
    [Header("Radar")]
    public Image radarButton;
    public float radarCooldown = 60;
    public GameObject radarMenu;

    private float radarTimer = 0;
    public bool isRadarOffCooldown = true;

    public Radar radar;
    public MapGenerator mapGenerator;
    public RectTransform minimapRadarOverlayRectTrans;

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
        isRadarOffCooldown = true;
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
            Debug.Log("attempting build");
            Vector3 MousePos = MousePosition.MouseToWorld3D(Camera.main, -1);
            MousePos.y = transform.position.y;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer))
            {
                Debug.Log("ray hit");
                if (hit.collider.CompareTag("Socket"))
                {
                    socket = hit.collider.GetComponent<SentrySocket>();
                    AssignSocket(socket);
                    if (!socket.HasSentry())
                    {
                        OpenBuildMenu();                        
                    }
                    else
                    {
                        ShowRemoveSentryBtn();
                    }
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
            else if (!overMenu)
            {
                foreach (SentrySocket socket in allSockets)
                {
                    if (!socket.HasSentry())
                    {
                        socket.meshRender.material = mats[0];
                    }
                }
                socket = null;
                AssignSocket(null);
                CloseBuildMenu();
                CloseRemoveSentryBtn();
            }
        }
        TickRadarCooldown();
        ControlRadarOverlay();
        RotateRadarOverlay();
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
        GameManager.Instance.PauseGame();
        augmentMenu.GetComponent<AugmentMenu>().CreateAugmentChoices(augments);
        augmentMenu.SetActive(true);
    }

    public void ShowAttributeChoices()
    {
        GameManager.Instance.PauseGame();
        attributeMenu.GetComponent<AttributeMenu>().SetAttributes();
        attributeMenu.SetActive(true);
    }

    public void RemoveAugmentChoices()
    {
        GameManager.Instance.ResumeGame();
        augmentMenu.SetActive(false);
    }

    public void RemoveAttributeChoices()
    {
        GameManager.Instance.ResumeGame();
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
        GameManager.Instance.PauseGame();
        shopMenu.SetActive(true);
        resourceContainer.SetActive(false);
        shopMenu.GetComponent<ShopMenu>().OpenMenu(shopManager);
    }

    public void CloseShopMenu()
    {
        GameManager.Instance.ResumeGame();
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

    private void TickRadarCooldown()
    {
        radarTimer += Time.deltaTime;
        if (radarTimer >= radarCooldown) isRadarOffCooldown = true;
        if (!isRadarOffCooldown)
        {
            radarButton.fillAmount = radarTimer/radarCooldown;
        }
        else
        {
            radarButton.fillAmount = 1;
        }
    }

    public void ToggleRadarMenu()
    {
        if (!isRadarOffCooldown) return;
        radarMenu.SetActive(!radarMenu.activeSelf);
    }

    public void ResetRadarCooldown()
    {
        radarTimer = 0;
        isRadarOffCooldown = false;
    }

    private void ControlRadarOverlay()
    {
        if (radar.scannedEvent == null)
        {
            radar.isActiveScan = false;
        }
        else if (radar.scannedEvent != null && mapGenerator.DistFromPlayer(radar.scannedEvent.tileObjectPtr) < mapGenerator.tileOffset)
        {
            radar.isActiveScan = false;
        }
        minimapRadarOverlayRectTrans.gameObject.SetActive(!(!radar.isActiveScan || mapGenerator.DistFromPlayer(radar.scannedEvent.tileObjectPtr) < mapGenerator.tileOffset * 5));
    }

    private void RotateRadarOverlay()
    {
        if (!minimapRadarOverlayRectTrans.gameObject.activeSelf || radar.scannedEvent == null) return;
        Vector3 tilePos = radar.scannedEvent.tileObjectPtr.transform.position;
        Vector3 playerPos = mapGenerator.player.transform.position;

        minimapRadarOverlayRectTrans.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(-(tilePos.x - playerPos.x), (tilePos.z - playerPos.z)) * Mathf.Rad2Deg);
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

    public void ShowRemoveSentryBtn()
    {
        removeSentryBtn.SetActive(true);
    }

    public void CloseRemoveSentryBtn()
    {
        removeSentryBtn.SetActive(false);
    }

    public void RemoveSentry()
    {
        if (socket)
            socket.DeleteTurret();
        CloseRemoveSentryBtn();
    }
}