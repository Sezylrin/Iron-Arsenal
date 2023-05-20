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
    // public Vector2 sizeScale = new Vector2(100, 100);
    public float sizeScale = 100f;
    public Vector3 startLocalPos;
    public float buttonOffset = 150f;
    public int maxButtonsInRow = 4;
    // public GameObject mapCanvas;
    private List<MapGenerator.SpawnableEvent> spawnableEventsList;
    private List<Button> buttonsList = new List<Button>();
    // private List<UnityAction> listeners = new List<UnityAction>();
    // Start is called before the first frame update
    void Start()
    {
        if (buttonOffset < sizeScale) Debug.LogWarning("Button Offset should be >= Size Scale or the buttons overlap");
        spawnableEventsList = mapGenerator.spawnableEventList;
        CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGenerator.EventTile eventTile = Scan(spawnableEventsList[0]);
            if (eventTile == null)
            {
                Debug.Log("No " + spawnableEventsList[0].eventToSpawn.name +" found");
            }
            else
            {
                Debug.Log(eventTile.tileObjectPtr.transform.position);
                Debug.Log(eventTile.tileObjectPtr.name);
            }
        }
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

        buttonObject.transform.SetParent(gameObject.transform, false);
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
        button.onClick.AddListener(ToggleRadarMenu);
        buttonsList.Add(button);
    }

    public void ToggleRadarMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private MapGenerator.EventTile Scan(MapGenerator.SpawnableEvent eventToSearchFor)
    {
        Debug.Log("Here");
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
        if (eventTile == null)
        {
            Debug.Log("Unluggy");
            return;
        }
        eventTile.isDiscovered = true;
        Debug.Log(eventTile.tileObjectPtr.transform.position);
        //TODO: Do UI things
    }

    private float DistFromPlayer(Vector3 pos) => Vector3.Distance(pos, mapGenerator.player.transform.position);

    private void OnDestroy()
    {
        foreach (var button in buttonsList)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}