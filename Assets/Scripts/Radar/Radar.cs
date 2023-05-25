using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject minimapRadarOverlay;
    private RectTransform minimapRadarOverlayRectTrans;
    public Transform radarButtonsContainer;
    // public Vector2 sizeScale = new Vector2(100, 100);
    public float sizeScale = 100f;
    public Vector3 startLocalPos;
    public float buttonOffset = 150f;
    public int maxButtonsInRow = 4;
    // public GameObject mapCanvas;
    private List<MapGenerator.SpawnableEvent> spawnableEventsList;
    private List<Button> buttonsList = new List<Button>();
    public MapGenerator.EventTile scannedEvent = null;
    public bool isActiveScan = false;
    // private List<UnityAction> listeners = new List<UnityAction>();
    // Start is called before the first frame update
    void Start()
    {
        minimapRadarOverlayRectTrans = minimapRadarOverlay.GetComponent<RectTransform>();
        if (buttonOffset < sizeScale) Debug.LogWarning("Button Offset should be >= Size Scale or the buttons overlap");
        spawnableEventsList = mapGenerator.spawnableEventList;
        CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CreateButtons()
    {
        TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
        startLocalPos = new Vector3(-(buttonOffset/2) * (maxButtonsInRow-1), 0, 0);
        for (int i = 0; i < spawnableEventsList.Count; i++)
        {
            CreateButton(i, resources);
        }
    }

    private void CreateButton(int i, TMP_DefaultControls.Resources resources)
    {
        GameObject buttonObject = TMP_DefaultControls.CreateButton(resources);

        buttonObject.transform.SetParent(radarButtonsContainer, false);
        RectTransform buttonRectTrans = buttonObject.GetComponent<RectTransform>();
        buttonRectTrans.sizeDelta = new Vector2(sizeScale, sizeScale);
        if (i < maxButtonsInRow)
        {
            // buttonRectTrans.localPosition = startLocalPos + new Vector3(buttonOffset * i, 0, 0);
            buttonRectTrans.localPosition = startLocalPos + new Vector3(buttonOffset * i, 0, 0);
        }
        else
        {
            buttonRectTrans.localPosition = startLocalPos + new Vector3(buttonOffset * (i % maxButtonsInRow), buttonOffset * (Mathf.FloorToInt(i/maxButtonsInRow)), 0);
        }
        // buttonRectTrans.localPosition = new Vector3(-600 + 150 * i, -500, 0);

        Destroy(buttonObject.transform.GetChild(0).gameObject);

        Button button = buttonObject.GetComponent<Button>();

        if (spawnableEventsList[i].eventMapTile.GetComponentInChildren<SpriteRenderer>())
        {
            button.GetComponent<Image>().sprite = spawnableEventsList[i].eventMapTile.GetComponentInChildren<SpriteRenderer>().sprite;
        }

        button.onClick.AddListener(() => RevealEvent(Scan(spawnableEventsList[i])));
        button.onClick.AddListener(RadarCanvasManager.Instance.ToggleRadarMenu);
        button.onClick.AddListener(RadarCanvasManager.Instance.ResetRadarCooldown);
        buttonsList.Add(button);
    }

    private MapGenerator.EventTile Scan(MapGenerator.SpawnableEvent eventToSearchFor)
    {
        Vector3 maxStartPos = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 closestEventPos = maxStartPos;
        //TODO: Cooldown
        foreach (var tile in mapGenerator.allEventTiles)
        {
            if (tile.Value.isEmpty) continue;
            if (!tile.Value.tileObjectPtr) continue;
            if (tile.Value.tileObjectPtr.name != eventToSearchFor.eventToSpawn.name + "(Clone)") continue;
            if (!(DistFromPlayer(tile.Key) < DistFromPlayer(closestEventPos))) continue;
            closestEventPos = tile.Key;
        }

        return (closestEventPos != maxStartPos) ? mapGenerator.allEventTiles[closestEventPos] : null;
    }

    private void RevealEvent(MapGenerator.EventTile eventTile)
    {
        scannedEvent = eventTile;
        if (scannedEvent == null)
        {
            Debug.Log("Unluggy");
            return;
        }
        scannedEvent.isDiscovered = true;
        Debug.Log(scannedEvent.tileObjectPtr.transform.position);
        //TODO: Do UI things
        mapGenerator.SpawnRadarHighlight(scannedEvent.tileObjectPtr.transform.position);
        isActiveScan = true;
        minimapRadarOverlay.SetActive(true);
    }

    private float DistFromPlayer(Vector3 pos) => Vector3.Distance(pos, mapGenerator.player.transform.position);

    private void OnDestroy()
    {
        foreach (var button in buttonsList)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // public class RadarCanvasManager : MonoBehaviour
    // {
    //     public static RadarCanvasManager Instance { get; private set; }
    //     public Image radarButton;
    //     public float cooldown = 60;
    //     public GameObject radarMenu;
    //
    //     private float radarTimer = 0;
    //     public bool isRadarOffCooldown = true;
    //
    //     private void Awake()
    //     {
    //         if (Instance != null)
    //         {
    //             DestroyImmediate(Instance);
    //         }
    //         else
    //         {
    //             Instance = this;
    //         }
    //     }
    //
    //     void Start()
    //     {
    //         isRadarOffCooldown = true;
    //     }
    //
    //     void Update()
    //     {
    //         TickRadarCooldown();
    //     }
    //
    //     private void TickRadarCooldown()
    //     {
    //         radarTimer += Time.deltaTime;
    //         if (radarTimer >= cooldown) isRadarOffCooldown = true;
    //         if (!isRadarOffCooldown)
    //         {
    //             radarButton.fillAmount = radarTimer/cooldown;
    //         }
    //         else
    //         {
    //             radarButton.fillAmount = 1;
    //         }
    //     }
    //
    //     public void ResetRadarCooldown()
    //     {
    //         radarTimer = 0;
    //         isRadarOffCooldown = false;
    //     }
    //
    //     public void ToggleRadarMenu()
    //     {
    //         if (!isRadarOffCooldown) return;
    //         radarMenu.SetActive(!radarMenu.activeSelf);
    //     }
    // }
}