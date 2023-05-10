using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public List<MapGenerator.SpawnableEvent> spawnableEventsList;
    // Start is called before the first frame update
    void Start()
    {
        spawnableEventsList = mapGenerator.spawnableEventList;
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
}
