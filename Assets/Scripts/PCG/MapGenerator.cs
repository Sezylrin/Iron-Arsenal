using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Serializable]
    public class ObjectTiles
    {
        public GameObject objectToSpawn;
        public float chanceToSpawn;
    }
    public List<ObjectTiles> objectTilesList = new List<ObjectTiles>();

    public GameObject groundTile;
    public GameObject emptyTile;
    // public ObjectTiles objectTiles;
    public GameObject player;
    public Transform groundTilesContainer;
    public Transform objectTilesContainer;

    // [Range(0, 50)] public float chanceOfNovacite;
    // [Range(0, 50)] public float chanceOfVoidstone;
    // [Range(0, 50)] public float chanceOfXenorium;

    [Header("Radius is how many tiles should spawn around the player")]
    public int radius = 10;
    [Header("Tile Offset is how big the tiles are so they don't overlap")]
    public int tileOffset = 1;

    private Vector3 startPos = Vector3.zero;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / tileOffset) * tileOffset;
    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / tileOffset) * tileOffset;

    private Dictionary<Vector3, Tile> allGroundTiles = new Dictionary<Vector3, Tile>();
    private Dictionary<Vector3, Tile> activeGroundTiles = new Dictionary<Vector3, Tile>();
    private Dictionary<Vector3, Tile> allObjectTiles = new Dictionary<Vector3, Tile>();
    private Dictionary<Vector3, Tile> activeObjectTiles = new Dictionary<Vector3, Tile>();

    private void Start()
    {
        GenerateTiles();
    }

    private void Update()
    {
        if (!PlayerHasMoved()) return;
        GenerateTiles();
    }

    private void GenerateTiles()
    {
        var newActiveGroundTiles = new Dictionary<Vector3, Tile>();
        var newActiveObjectTiles = new Dictionary<Vector3, Tile>();
        float cTime = Time.realtimeSinceStartup;

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                Vector3 pos = new Vector3(x * tileOffset + XPlayerLocation, 0, z * tileOffset + ZPlayerLocation);

                // If a ground tile hasn't spawned in that position yet
                if (!allGroundTiles.ContainsKey(pos))
                {
                    SpawnNewTile(pos, cTime);
                }
                // If a tile occupies that position
                else
                {
                    // Update the timestamp to be the current time
                    allGroundTiles[pos].cTimestamp = cTime;
                    allObjectTiles[pos].cTimestamp = cTime;
                    // If the tile is not active
                    if (!activeGroundTiles.ContainsKey(pos))
                    {
                        // Activate that tile
                        allGroundTiles[pos].tileObject.SetActive(true);
                        activeGroundTiles.Add(pos, allGroundTiles[pos]);
                    }
                    if (!activeObjectTiles.ContainsKey(pos))
                    {
                        if (allObjectTiles[pos].tileObject && allObjectTiles[pos].tileObject != emptyTile)
                        {
                            allObjectTiles[pos].tileObject.SetActive(true);
                            activeObjectTiles.Add(pos, allObjectTiles[pos]);
                        }
                    }
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

        foreach (var tile in activeObjectTiles)
        {
            // If they aren't in bounds of current time
            if (!tile.Value.cTimestamp.Equals(cTime))
            {
                if (tile.Value.tileObject && tile.Value.tileObject != emptyTile)
                {
                    // Disable object
                    tile.Value.tileObject.SetActive(false);
                }
            }
            else
            {
                newActiveObjectTiles.Add(tile.Key, tile.Value);
            }
        }

        activeGroundTiles = newActiveGroundTiles;
        activeObjectTiles = newActiveObjectTiles;
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
        Tile tile = new Tile(cTime, tileInstance);
        activeGroundTiles.Add(pos, tile);
        allGroundTiles.Add(pos, tile);
    }

    private void SpawnNewObjectTile(Vector3 pos, float cTime)
    {
        GameObject tileType = RandomTile();
        GameObject tileInstance;
        if (!tileType)
        {
            tileInstance = emptyTile;
        }
        else
        {
            tileInstance = Instantiate(tileType, pos + new Vector3(0.0f,0.01f,0.0f), Quaternion.identity, objectTilesContainer);
        }
        Tile tile = new Tile(cTime, tileInstance);
        activeObjectTiles.Add(pos, tile);
        allObjectTiles.Add(pos, tile);
    }

    private GameObject RandomTile()
    {
        int random = Random.Range(0, 100);
        GameObject tileType = null;

        // Old code just in case
        // if (0 < random && random <= chanceOfNovacite)
        // {
        //     tileType = objectTiles.novacite;
        // }
        // if (chanceOfNovacite < random && random <= chanceOfVoidstone + chanceOfNovacite)
        // {
        //     tileType = objectTiles.voidstone;
        // }
        // if (chanceOfVoidstone + chanceOfNovacite < random && random <= chanceOfNovacite + chanceOfVoidstone + chanceOfXenorium)
        // {
        //     tileType = objectTiles.xenorium;
        // }

        for (int i = 0; i < objectTilesList.Count; i++)
        {
            float lowerBound = 0;
            float upperBound;
            for (int j = 0; j < i; j++)
            {
                lowerBound += objectTilesList[j].chanceToSpawn;
            }
            upperBound = objectTilesList[i].chanceToSpawn + lowerBound;
            if (lowerBound <= random && random < upperBound)
            {
                tileType = objectTilesList[i].objectToSpawn;
            }
        }

        return tileType;
    }

    private class Tile
    {
        public float cTimestamp;
        public GameObject tileObject;

        public Tile(float cTimestamp, GameObject tileObject)
        {
            this.cTimestamp = cTimestamp;
            this.tileObject = tileObject;
        }
    }

    private bool PlayerHasMoved() => Mathf.Abs(XPlayerMove) >= tileOffset || Mathf.Abs(ZPlayerMove) >= tileOffset;
}