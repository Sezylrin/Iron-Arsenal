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
    public Vector2 sizeScale = new Vector2(100, 100);
    public Vector3 startLocalPos = new Vector3(-600, -500, 0);
    public float buttonOffset = 150f;
    public int maxButtonsInRow = 4;
    // public GameObject mapCanvas;
    private List<MapGenerator.SpawnableEvent> spawnableEventsList;
    private Button button;
    private List<Button> buttonsList = new List<Button>();
    // private List<UnityAction> listeners = new List<UnityAction>();
    // Start is called before the first frame update
    void Start()
    {
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
                Debug.Log(eventTile.tileObject.transform.position);
                Debug.Log(eventTile.tileObject.name);
            }
        }
    }

    private void CreateButtons()
    {
        TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
        Debug.Log(spawnableEventsList.Count);
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
        buttonRectTrans.sizeDelta = sizeScale;
        if (i < maxButtonsInRow)
        {
            buttonRectTrans.localPosition = startLocalPos + new Vector3(buttonOffset * i, 0, 0);
        }
        else
        {
            buttonRectTrans.localPosition = startLocalPos + new Vector3(buttonOffset * (i - maxButtonsInRow), -buttonOffset, 0);
        }
        // buttonRectTrans.localPosition = new Vector3(-600 + 150 * i, -500, 0);

        Destroy(buttonObject.transform.GetChild(0).gameObject);

        button = buttonObject.GetComponent<Button>();

        if (spawnableEventsList[i].eventMapTile.GetComponentInChildren<SpriteRenderer>())
        {
            button.GetComponent<Image>().sprite = spawnableEventsList[i].eventMapTile.GetComponentInChildren<SpriteRenderer>().sprite;
        }

        button.onClick.AddListener(() => Scan(spawnableEventsList[i]));
        buttonsList.Add(button);
    }

    public void OnButton() => Scan(spawnableEventsList[0]);

    private MapGenerator.EventTile Scan(MapGenerator.SpawnableEvent eventToSearchFor)
    {
        Vector3 maxStartPos = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 closestEventPos = maxStartPos;
        // int total = 0;
        foreach (var tile in mapGenerator.allEventTiles)
        {
            if (tile.Value.isEmpty) continue;
            if (!tile.Value.tileObject) continue;
            if (tile.Value.tileObject.name == eventToSearchFor.eventToSpawn.name + "(Clone)")
                // total++;
                if (DistFromPlayer(tile.Key) < DistFromPlayer(closestEventPos))
                    closestEventPos = tile.Key;
        }

        // Debug.Log(total);
        // Debug.Log(closestEventPos);
        return (closestEventPos != maxStartPos) ? mapGenerator.allEventTiles[closestEventPos] : null;
    }

    private float DistFromPlayer(Vector3 pos) => Vector3.Distance(pos, mapGenerator.player.transform.position);

    private void OnDisable() => button.onClick.RemoveAllListeners();
}