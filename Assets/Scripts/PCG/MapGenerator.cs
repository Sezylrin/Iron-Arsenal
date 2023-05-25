using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("Spawned tiles")]
    public GameObject groundTile;
    public GameObject groundMapTile;
    public GameObject undiscoveredGroundMapTile;
    public GameObject undiscoveredEventMapTile;
    public GameObject radarMapTileHighlight;

    [Serializable]
    public class SpawnableEvent
    {
        public GameObject eventToSpawn;
        public float chanceToSpawn;
        public GameObject eventMapTile;
    }
    public List<SpawnableEvent> spawnableEventList = new List<SpawnableEvent>();

    [Header("For testing, should auto grab player if LevelManager in scene")] public GameObject player;
    [Header("Containers: Drag the corresponding containers into these fields if empty")]
    public Transform groundTilesContainer;
    public Transform eventTilesContainer;
    public Transform mapTilesContainer;
    public Transform undiscoveredMapTilesContainer;
    public Transform miscContainer;

    [Header("How many tiles away from the player that ground will spawn")]
    public int groundRadius = 10;
    [Header("How many tiles away from the player that events will spawn")]
    public int eventRadius = 20;
    [Header("How big the tiles are so they don't overlap")]
    public int tileOffset = 9;
    [Header("Tiles between event spawns")]
    public int minTilesBtwnEvents;

    public bool spawnTiles = true;

    private Vector3 startPos = Vector3.zero;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / tileOffset) * tileOffset;
    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / tileOffset) * tileOffset;

    private Dictionary<Vector3, GroundTile> allGroundTiles = new Dictionary<Vector3, GroundTile>();
    private Dictionary<Vector3, GroundTile> activeGroundTiles = new Dictionary<Vector3, GroundTile>();


    public Dictionary<Vector3, EventTile> allEventTiles = new Dictionary<Vector3, EventTile>();
    private Dictionary<Vector3, EventTile> groundEventTiles = new Dictionary<Vector3, EventTile>();

    private void Start()
    {
        if (!player)
        {
            player = LevelManager.Instance.player;
        }
        if (!spawnTiles) return;
        GenerateTiles();
        // UpdateActiveTiles();
        UpdateMapTiles();
    }

    private void Update()
    {
        if (!spawnTiles) return;
        UpdateMapTiles();
        if (!PlayerHasMoved()) return;
        GenerateTiles();
        startPos = player.transform.position;
    }

    private void GenerateTiles()
    {
        float cTime = Time.realtimeSinceStartup;

        for (int x = -groundRadius; x <= groundRadius; x++)
        {
            for (int z = -groundRadius; z <= groundRadius; z++)
            {
                Vector3 pos = new Vector3(x * tileOffset + XPlayerLocation, 0, z * tileOffset + ZPlayerLocation);

                // If a ground tile hasn't spawned in that position yet
                if (!allGroundTiles.ContainsKey(pos))
                {
                    SpawnNewGroundTile(pos, cTime);
                    EventTile newGroundEventTile = SpawnNewGroundEventTile(pos);
                    SpawnNewMapTile(pos, newGroundEventTile);
                }
                // If a tile occupies that position
                else
                {
                    // Update the timestamp to be the current time
                    allGroundTiles[pos].cTimestamp = cTime;
                    // If the tile is not active
                    if (!activeGroundTiles.ContainsKey(pos))
                    {
                        // Activate that tile
                        allGroundTiles[pos].tileObjectPtr.SetActive(true);
                        activeGroundTiles.Add(pos, allGroundTiles[pos]);
                    }
                }
            }
        }

        for (int x = -eventRadius; x <= eventRadius; x++)
        {
            for (int z = -eventRadius; z <= eventRadius; z++)
            {
                Vector3 pos = new Vector3(x * tileOffset + XPlayerLocation, 0, z * tileOffset + ZPlayerLocation);

                if (!allEventTiles.ContainsKey(pos))
                {
                    EventTile newEventTile = SpawnNewEventTile(pos);
                    SpawnNewMapTile(pos, newEventTile);
                }
            }
        }

        var newActiveGroundTiles = new Dictionary<Vector3, GroundTile>();

        // Loop through all active tiles
        foreach (var tile in activeGroundTiles)
        {
            // If they aren't in bounds of current time
            if (!tile.Value.cTimestamp.Equals(cTime))
            {
                // Disable object
                tile.Value.tileObjectPtr.SetActive(false);
            }
            else
            {
                newActiveGroundTiles.Add(tile.Key, tile.Value);
            }
        }

        activeGroundTiles = newActiveGroundTiles;
    }

    private void UpdateMapTiles()
    {
        foreach (var tile in allEventTiles)
        {
            // Destroy mapTiles of completed events
            if (!tile.Value.tileObjectPtr && !tile.Value.isEmpty && tile.Value.eventMapTilePtr)
            {
                Destroy(tile.Value.eventMapTilePtr);
            }

            if (tile.Value.eventMapTilePtr && DistFromPlayer(tile.Value.eventMapTilePtr) < groundRadius * tileOffset)
            {
                tile.Value.isDiscovered = true;
            }

            if (tile.Value.isDiscovered && tile.Value.undiscoveredMapTilePtr)
            {
                Destroy(tile.Value.undiscoveredMapTilePtr);
            }
        }

        foreach (var tile in groundEventTiles)
        {
            if (tile.Value.eventMapTilePtr && DistFromPlayer(tile.Value.eventMapTilePtr) < groundRadius * tileOffset)
            {
                tile.Value.isDiscovered = true;
            }

            if (tile.Value.isDiscovered && tile.Value.undiscoveredMapTilePtr)
            {
                Destroy(tile.Value.undiscoveredMapTilePtr);
            }
        }
    }


    private void SpawnNewGroundTile(Vector3 pos, float cTime)
    {
        GameObject tileInstance = Instantiate(groundTile, pos, Quaternion.identity, groundTilesContainer);
        GroundTile tile = new GroundTile(cTime, tileInstance);
        activeGroundTiles.Add(pos, tile);
        allGroundTiles.Add(pos, tile);
    }

    private EventTile SpawnNewGroundEventTile(Vector3 pos)
    {
        EventTile tile = new EventTile();
        groundEventTiles.Add(pos, tile);
        return tile;
    }

    private EventTile SpawnNewEventTile(Vector3 pos)
    {
        SpawnableEvent tileType = RandomEventTile();
        EventTile tile = new EventTile();
        if (tileType != null && !EventIsNearOtherEvents(pos) && pos != Vector3.zero)
        {
            GameObject tileInstance = Instantiate(tileType.eventToSpawn, pos + new Vector3(0.0f,1.51f,0.0f), Quaternion.identity, eventTilesContainer);
            tile = new EventTile(tileInstance, tileType.eventMapTile);
        }

        allEventTiles.Add(pos, tile);

        return tile;
    }

    private void SpawnNewMapTile(Vector3 pos, EventTile eventTile)
    {
        GameObject mapTileType = (eventTile.isEmpty) ? groundMapTile : eventTile.eventMapTileRef;
        if (!eventTile.isEmpty) pos += new Vector3(0.0f, 0.01f, 0.0f);
        eventTile.eventMapTilePtr = Instantiate(mapTileType, pos, Quaternion.identity, mapTilesContainer);

        pos += new Vector3(0.0f, 0.01f, 0.0f);
        GameObject undiscoveredMapTileType = eventTile.isEmpty ? undiscoveredGroundMapTile : undiscoveredEventMapTile;
        if (!eventTile.isEmpty) pos += new Vector3(0.0f, 0.01f, 0.0f);
        eventTile.undiscoveredMapTilePtr = Instantiate(undiscoveredMapTileType, pos, Quaternion.identity, undiscoveredMapTilesContainer);
    }

    private SpawnableEvent RandomEventTile()
    {
        int random = Random.Range(0, 1000);
        SpawnableEvent tileType = null;
        for (int i = 0; i < spawnableEventList.Count; i++)
        {
            float lowerBound = 0;
            for (int j = 0; j < i; j++)
            {
                lowerBound += spawnableEventList[j].chanceToSpawn;
            }
            float upperBound = spawnableEventList[i].chanceToSpawn + lowerBound;
            if (lowerBound <= random && random < upperBound)
            {
                tileType = spawnableEventList[i];
            }
        }

        return tileType;
    }

    public void SpawnRadarHighlight(Vector3 pos)
    {
        GameObject obj = Instantiate(radarMapTileHighlight, pos, Quaternion.identity, miscContainer);
        obj.GetComponent<RadarMapTileHighlight>().mapGenerator = this;
    }

    private class GroundTile
    {
        public float cTimestamp;
        public GameObject tileObjectPtr;

        public GroundTile(float cTimestamp, GameObject tileObjectPtr)
        {
            this.cTimestamp = cTimestamp;
            this.tileObjectPtr = tileObjectPtr;
        }
    }

    public class EventTile
    {
        public bool isEmpty;
        public bool isDiscovered;
        public GameObject tileObjectPtr;
        public GameObject eventMapTileRef;
        public GameObject eventMapTilePtr;
        public GameObject undiscoveredMapTilePtr;

        public EventTile()
        {
            isEmpty = true;
            isDiscovered = false;
        }
        public EventTile(GameObject tileObjectPtr, GameObject eventMapTileRef)
        {
            isEmpty = false;
            isDiscovered = false;
            this.tileObjectPtr = tileObjectPtr;
            this.eventMapTileRef = eventMapTileRef;
        }
    }

    private bool PlayerHasMoved() => Mathf.Abs(XPlayerMove) >= tileOffset || Mathf.Abs(ZPlayerMove) >= tileOffset;

    private bool EventIsNearOtherEvents (Vector3 pos)
    {
        for (int i = 1; i < minTilesBtwnEvents; i++)
        {
            for (int x = -i; x <= i; x++)
            {
                for (int z = -i; z <= i; z++)
                {
                    if (x == 0 && z == 0) continue;
                    if (Mathf.Abs(x) + Mathf.Abs(z) > i) continue;
                    Vector3 posToCheck = pos + new Vector3(tileOffset * x, 0, tileOffset * z);
                    if (allEventTiles.ContainsKey(posToCheck) && !allEventTiles[posToCheck].isEmpty)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public float DistFromPlayer(GameObject obj)
    {
        return Vector3.Distance( new Vector3(obj.transform.position.x, 0, obj.transform.position.z),
            new Vector3(player.transform.position.x, 0, player.transform.position.z));
    }
}