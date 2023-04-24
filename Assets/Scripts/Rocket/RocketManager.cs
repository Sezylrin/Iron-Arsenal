using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
    // TODO: THIS SHOULD BE COMBINED INTO THE LEVELMANAGER OR SIMILAR
    public static int rocketPartsCollected { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void collectRocketPart() => rocketPartsCollected++;
}