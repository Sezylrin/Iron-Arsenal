using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadarCanvasManager : MonoBehaviour
{
    public static RadarCanvasManager Instance { get; private set; }
    public Image radarButton;
    public float cooldown = 60;
    public GameObject radarMenu;

    private float radarTimer = 0;
    public bool isRadarOffCooldown = true;

    public Radar radar;
    public MapGenerator mapGenerator;
    public RectTransform minimapRadarOverlayRectTrans;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isRadarOffCooldown = true;
    }

    void Update()
    {
        TickRadarCooldown();
        ControlRadarOverlay();
        RotateRadarOverlay();
    }

    private void TickRadarCooldown()
    {
        radarTimer += Time.deltaTime;
        if (radarTimer >= cooldown) isRadarOffCooldown = true;
        if (!isRadarOffCooldown)
        {
            radarButton.fillAmount = radarTimer/cooldown;
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
        if (radar.scannedEvent == null || mapGenerator.DistFromPlayer(radar.scannedEvent.tileObjectPtr) < mapGenerator.tileOffset) radar.isActiveScan = false;
        minimapRadarOverlayRectTrans.gameObject.SetActive(!(!radar.isActiveScan || mapGenerator.DistFromPlayer(radar.scannedEvent.tileObjectPtr) < mapGenerator.tileOffset * 5));
    }

    private void RotateRadarOverlay()
    {
        if (!minimapRadarOverlayRectTrans.gameObject.activeSelf || radar.scannedEvent == null) return;
        Vector3 tilePos = radar.scannedEvent.tileObjectPtr.transform.position;
        Vector3 playerPos = mapGenerator.player.transform.position;

        minimapRadarOverlayRectTrans.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(-(tilePos.x - playerPos.x), (tilePos.z - playerPos.z)) * Mathf.Rad2Deg);
    }
}