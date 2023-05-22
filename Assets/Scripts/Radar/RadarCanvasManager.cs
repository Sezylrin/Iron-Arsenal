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

    public void ResetRadarCooldown()
    {
        radarTimer = 0;
        isRadarOffCooldown = false;
    }

    public void ToggleRadarMenu()
    {
        if (!isRadarOffCooldown) return;
        radarMenu.SetActive(!radarMenu.activeSelf);
    }
}