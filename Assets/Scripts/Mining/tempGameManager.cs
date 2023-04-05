using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGameManager : MonoBehaviour
{
    public int[] ores; // 0 - Iron, 1 - Copper, 2 - Gold

    // Start is called before the first frame update
    void Start()
    {
        ores = new int[3];
        ores[0] = 0;
        ores[1] = 0;
        ores[2] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
