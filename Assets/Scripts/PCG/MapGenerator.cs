using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Serializable]
    public struct Tiles
    {
        public GameObject grass;
        public GameObject water;
        public GameObject dirt;
    }

    public Tiles tiles;
    public GameObject player;
    public Transform container;

    [Range(0, 50)] public float chanceOfWater;
    [Range(0, 50)] public float chanceOfDirt;

    [Header("Values for Tiles to not overlap")]
    public int radius = 10;
    public int tileOffset = 1;

    private Vector3 startPos = Vector3.zero;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / tileOffset) * tileOffset;
    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / tileOffset) * tileOffset;

    private Dictionary<Vector3, Tile> allTiles = new Dictionary<Vector3, Tile>();
    private Dictionary<Vector3, Tile> activeTiles = new Dictionary<Vector3, Tile>();

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
        var newActiveTiles = new Dictionary<Vector3, Tile>();
        float cTime = Time.realtimeSinceStartup;

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                Vector3 pos = new Vector3(x * tileOffset + XPlayerLocation, 0, z * tileOffset + ZPlayerLocation);

                // If a tile hasn't spawned in that position yet
                if (!allTiles.ContainsKey(pos))
                {
                    var random = Random.Range(0.0f, 100.0f);
                    GameObject plane = tiles.grass;
                    if (random > 0 && random <= chanceOfDirt)
                    {
                        plane = tiles.dirt;
                    }
                    if (random > chanceOfDirt && random <= chanceOfDirt + chanceOfWater)
                    {
                        plane = tiles.water;
                    }
                    GameObject tileInstance = Instantiate(plane, pos, Quaternion.identity, container);
                    Tile tile = new Tile(cTime, tileInstance);
                    activeTiles.Add(pos, tile);
                    allTiles.Add(pos, tile);
                }
                // If a tile has occupies that position
                else
                {
                    // Update the timestamp to be the current time
                    allTiles[pos].cTimestamp = cTime;
                    // If the tile is not active
                    if (!activeTiles.ContainsKey(pos))
                    {
                        allTiles[pos].tileObject.SetActive(true);
                        activeTiles.Add(pos, allTiles[pos]);
                    }
                }
            }
        }

        // Loop through all active tiles
        foreach (var tile in activeTiles)
        {
            // If they aren't in bounds of current time
            if (!tile.Value.cTimestamp.Equals(cTime))
            {
                // Disable object
                tile.Value.tileObject.SetActive(false);
            }
            else
            {
                newActiveTiles.Add(tile.Key, tile.Value);
            }
        }

        activeTiles = newActiveTiles;
        startPos = player.transform.position;
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