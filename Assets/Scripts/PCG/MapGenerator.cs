using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    // [Serializable]
    // public struct ObjectTiles
    // {
    //     public GameObject novacite;
    //     public GameObject voidstone;
    //     public GameObject xenorium;
    // }
    [Header("Spawned tiles")]
    public GameObject groundTile;
    public GameObject groundMapTile;

    [Serializable]
    public class EventTiles
    {
        public GameObject eventToSpawn;
        public float chanceToSpawn;
        public int DCToStartSpawning;
        public GameObject eventMapTile;
    }
    public List<EventTiles> eventTilesList = new List<EventTiles>();
    private int eventTilesListEle = -1;

    // public GameObject emptyTile;
    // public ObjectTiles objectTiles;

    [SerializeField, Header("For testing, should auto grab player if LevelManager in scene")] private GameObject player;
    [Header("Containers: Drag the corresponding siblings into these fields if empty")]
    public Transform groundTilesContainer;
    public Transform eventTilesContainer;
    public Transform mapTilesContainer;

    [Header("How far away from the player that ground will spawn")]
    public int groundRadius = 10;
    [Header("How far away from the player that events will spawn")]
    public int eventRadius = 20;
    [Header("How big the tiles are so they don't overlap")]
    public int tileOffset = 9;
    [Header("Tiles between event spawns")]
    public int minTilesBtwnEvents;
    [Header("Turn this on in scene with an enemy manager to activate DCToStartSpawning")]
    public bool enemyManagerInScene = false;

    private Vector3 startPos = Vector3.zero;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / tileOffset) * tileOffset;
    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / tileOffset) * tileOffset;

    private Dictionary<Vector3, GroundTile> allGroundTiles = new Dictionary<Vector3, GroundTile>();
    private Dictionary<Vector3, GroundTile> activeGroundTiles = new Dictionary<Vector3, GroundTile>();
    private Dictionary<Vector3, EventTile> allEventTiles = new Dictionary<Vector3, EventTile>();
    // private Dictionary<Vector3, EventTile> activeEventTiles = new Dictionary<Vector3, EventTile>();

    private void Awake()
    {
        if (!player)
        {
            player = LevelManager.Instance.player;
        }
    }

    private void Start()
    {
        GenerateTiles();
        // GameObject tileInstance = Instantiate(emptyTile, new Vector3(0.0f,0.01f,0.0f), Quaternion.identity, objectTilesContainer);
        // EventTile tile = new EventTile(tileInstance, true);
        // allEventTiles[Vector3.zero] = tile;
    }

    private void Update()
    {
        if (!PlayerHasMoved()) return;
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        var newActiveGroundTiles = new Dictionary<Vector3, GroundTile>();
        var newActiveObjectTiles = new Dictionary<Vector3, GroundTile>();
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
                    SpawnNewMapObject(pos);
                }
                // If a tile occupies that position
                else
                {
                    // Update the timestamp to be the current time
                    allGroundTiles[pos].cTimestamp = cTime;
                    // allObjectTiles[pos].cTimestamp = cTime;
                    // If the tile is not active
                    if (!activeGroundTiles.ContainsKey(pos))
                    {
                        // Activate that tile
                        allGroundTiles[pos].tileObject.SetActive(true);
                        activeGroundTiles.Add(pos, allGroundTiles[pos]);
                    }
                    // if (!activeObjectTiles.ContainsKey(pos))
                    // {
                    //     if (allObjectTiles[pos].tileObject && allObjectTiles[pos].tileObject != emptyTile)
                    //     {
                    //         allObjectTiles[pos].tileObject.SetActive(true);
                    //         activeObjectTiles.Add(pos, allObjectTiles[pos]);
                    //     }
                    // }
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
                    SpawnNewObjectTile(pos, cTime);
                    SpawnNewMapObject(pos);
                }
            }
        }

        // Loop through all active tiles
        foreach (var tile in activeGroundTiles)
        {
            // If they aren't in bounds of current time
            if (!tile.Value.cTimestamp.Equals(cTime))
            {
                // Disable object
                tile.Value.tileObject.SetActive(false);
            }
            else
            {
                newActiveGroundTiles.Add(tile.Key, tile.Value);
            }
        }

        // foreach (var tile in activeEventTiles)
        // {
        //     // If they aren't in bounds of current time
        //     if (!tile.Value.cTimestamp.Equals(cTime))
        //     {
        //         if (tile.Value.tileObject && tile.Value.tileObject != emptyTile)
        //         {
        //             // Disable object
        //             tile.Value.tileObject.SetActive(false);
        //         }
        //     }
        //     else
        //     {
        //         newActiveObjectTiles.Add(tile.Key, tile.Value);
        //     }
        // }

        activeGroundTiles = newActiveGroundTiles;
        // activeEventTiles = newActiveObjectTiles;
        startPos = player.transform.position;
    }

    private void SpawnNewTile(Vector3 pos, float cTime)
    {
        SpawnNewGroundTile(pos, cTime);

        SpawnNewObjectTile(pos, cTime);
    }

    private void SpawnNewGroundTile(Vector3 pos, float cTime)
    {
        GameObject tileInstance = Instantiate(groundTile, pos, Quaternion.identity, groundTilesContainer);
        GroundTile tile = new GroundTile(cTime, tileInstance);
        activeGroundTiles.Add(pos, tile);
        allGroundTiles.Add(pos, tile);
    }

    private void SpawnNewObjectTile(Vector3 pos, float cTime)
    {
        GameObject tileType = RandomTile();
        EventTile tile = new EventTile();
        if (tileType && !EventIsNearOtherEvents(pos))
        {
            GameObject tileInstance = Instantiate(tileType, pos + new Vector3(0.0f,0.01f,0.0f), Quaternion.identity, eventTilesContainer);
            tile = new EventTile(tileInstance);
        }
        // activeEventTiles.Add(pos, tile);
        allEventTiles.Add(pos, tile);
    }

    private void SpawnNewMapObject(Vector3 pos)
    {
        if (eventTilesListEle < 0)
        {
            // Spawn ground mapObj
            Instantiate(groundMapTile, pos, Quaternion.identity, mapTilesContainer);
        }
        else
        {
            // Spawn event mapObj
            Instantiate(eventTilesList[eventTilesListEle].eventMapTile, pos, Quaternion.identity, mapTilesContainer);
        }

    }

    private GameObject RandomTile()
    {
        int random = Random.Range(0, 1000);
        GameObject tileType = null;
        eventTilesListEle = -1;

        for (int i = 0; i < eventTilesList.Count; i++)
        {
            float lowerBound = 0;
            for (int j = 0; j < i; j++)
            {
                lowerBound += eventTilesList[j].chanceToSpawn;
            }
            float upperBound = eventTilesList[i].chanceToSpawn + lowerBound;
            if (lowerBound <= random && random < upperBound || enemyManagerInScene && EnemyManager.Instance.Difficulty >= eventTilesList[i].DCToStartSpawning)
            {
                tileType = eventTilesList[i].eventToSpawn;
                eventTilesListEle = i;
            }
        }

        return tileType;
    }

    private class GroundTile
    {
        public float cTimestamp;
        public GameObject tileObject;

        public GroundTile(float cTimestamp, GameObject tileObject)
        {
            this.cTimestamp = cTimestamp;
            this.tileObject = tileObject;
        }
    }

    private class EventTile
    {
        public bool isEmpty;
        public GameObject tileObject;

        public EventTile()
        {
            isEmpty = true;
        }

        public EventTile(GameObject tileObject)
        {
            isEmpty = false;
            this.tileObject = tileObject;
        }
    }

    private bool PlayerHasMoved() => Mathf.Abs(XPlayerMove) >= tileOffset || Mathf.Abs(ZPlayerMove) >= tileOffset;

    private bool EventIsNearOtherEvents (Vector3 pos)
    {
        for (int i = 1; i <= minTilesBtwnEvents; i++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;
                    if (Mathf.Abs(x) != Mathf.Abs(z))
                    {
                        Vector3 posToCheck = pos + new Vector3(tileOffset * x, 0, tileOffset * z) * i;
                        if (allEventTiles.ContainsKey(posToCheck) && !allEventTiles[posToCheck].isEmpty)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Vector3 posToCheck = pos + new Vector3(tileOffset * x, 0, tileOffset * z) * (i-1);
                        if (allEventTiles.ContainsKey(posToCheck) && !allEventTiles[posToCheck].isEmpty)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}