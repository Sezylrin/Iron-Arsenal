using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarMapTileHighlight : MonoBehaviour
{
    public MapGenerator mapGenerator;

    // Update is called once per frame
    void Update()
    {
        if (!mapGenerator) return;
        if (mapGenerator.DistFromPlayer(gameObject) < mapGenerator.tileOffset)
        {
            Destroy(gameObject);
        }
    }
}