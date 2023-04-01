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
    public int totalResourcesInDeposit;
    public int currentResourcesInDeposit;

    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<tempPlayer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<tempGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalResourcesInDeposit = Random.Range(100, 501);
        currentResourcesInDeposit = totalResourcesInDeposit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mine()
    {
        if (type == OreType.Ore1) 
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                gameManager.ore1 += currentResourcesInDeposit;
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                gameManager.ore1 += playerScript.miningOutput;
            }
        }
        else if (type == OreType.Ore2)
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                gameManager.ore2 += currentResourcesInDeposit;
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                gameManager.ore2 += playerScript.miningOutput;
            }
        }
        else if (type == OreType.Ore3)
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                gameManager.ore3 += currentResourcesInDeposit;
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                gameManager.ore3 += playerScript.miningOutput;
            }
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
