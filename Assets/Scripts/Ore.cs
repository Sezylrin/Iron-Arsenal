using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public enum OreType
    {
        Ore1,
        Ore2,
        Ore3
    };

    public OreType type;
    private tempPlayer playerScript;
    private tempGameManager gameManager;

    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<tempPlayer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<tempGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mine()
    {
        if (type == OreType.Ore1) 
        {
            gameManager.ore1 += playerScript.miningOutput;
        }
        else if (type == OreType.Ore2)
        {
            gameManager.ore2 += playerScript.miningOutput;
        }
        else if (type == OreType.Ore3)
        {
            gameManager.ore3 += playerScript.miningOutput;
        }
    }

    public void startMining()
    {
        InvokeRepeating("mine", 0f, playerScript.miningSpeed);
    }

    public void stopMining()
    {
        CancelInvoke("mine");
    }
}
