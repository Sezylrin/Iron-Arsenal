using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public enum OreType
    {
        Iron,
        Copper,
        Gold
    };

    public OreType type;
    private tempPlayer playerScript;
    private LevelManager levelManager;
    public int totalResourcesInDeposit;
    public int currentResourcesInDeposit;

    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<tempPlayer>();
        levelManager = LevelManager.Instance;
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
        if (type == OreType.Iron) 
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                levelManager.GainIron(currentResourcesInDeposit);
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                levelManager.GainIron(playerScript.miningOutput);
            }
        }
        else if (type == OreType.Copper)
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                levelManager.GainCopper(currentResourcesInDeposit);
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                levelManager.GainCopper(playerScript.miningOutput);
            }
        }
        else if (type == OreType.Gold)
        {
            if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
            {
                levelManager.GainGold(currentResourcesInDeposit);
                Destroy(gameObject);
            }
            else
            {
                currentResourcesInDeposit -= playerScript.miningOutput;
                levelManager.GainGold(playerScript.miningOutput);
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
