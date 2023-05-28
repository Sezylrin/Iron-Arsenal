using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarOverlay : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MapGenerator.EventTile scannedTile;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapGenerator) return;
        if (mapGenerator.DistFromPlayer(scannedTile.tileObjectPtr) < mapGenerator.tileOffset * 5)
        {
            gameObject.SetActive(false);
        }
    }
}