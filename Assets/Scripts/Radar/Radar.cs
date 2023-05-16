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
    public GameObject mapCanvas;
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
            GameObject buttonObject = TMP_DefaultControls.CreateButton(resources);
            buttonObject.transform.SetParent(mapCanvas.transform, false);
            RectTransform buttonRectTrans = buttonObject.GetComponent<RectTransform>();
            buttonRectTrans.sizeDelta = new Vector2(100, 100);
            buttonRectTrans.localPosition = new Vector3(-600 + 150 * i, -500, 0);
            Destroy(buttonObject.transform.GetChild(0).gameObject);
            button = buttonObject.GetComponent<Button>();
            // listeners.Add(() => Scan(spawnableEventsList[0]));
            // button.onClick.AddListener(OnButton);
            var i1 = i;
            button.onClick.AddListener(() => Scan(spawnableEventsList[i1]));
            buttonsList.Add(button);
        }
    }

    public void OnButton()
    {
        Scan(spawnableEventsList[0]);
    }

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

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}