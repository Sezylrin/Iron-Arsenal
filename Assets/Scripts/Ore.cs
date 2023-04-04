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
    private tempGameManager gameManager;
    public int totalResourcesInDeposit;
    public int currentResourcesInDeposit;

    public bool inOreTrigger;
    public bool ableToMine;
    public bool mining;

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
        inOreTrigger = false;
        ableToMine = true;
        mining = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (inOreTrigger && ableToMine && !mining)
            {
                mining = true;
                ableToMine = false;
                StartMining();
            }
        }
    }

    public void Mine()
    {
        int i = -1;
        switch (type)
        {
            case OreType.Iron:
                i = 0;
                break;
            case OreType.Copper:
                i = 1;
                break;
            case OreType.Gold:
                i = 2;
                break;
        }
            
        if (currentResourcesInDeposit - playerScript.miningOutput <= 0)
        {
            gameManager.ores[i] += currentResourcesInDeposit;
            Destroy(gameObject);
        }
        else
        {
            currentResourcesInDeposit -= playerScript.miningOutput;
            gameManager.ores[i] += playerScript.miningOutput;
        }
    }

    public void StartMining()
    {
        InvokeRepeating("Mine", 0f, playerScript.miningSpeed);
    }

    public void StopMining()
    {
        if (mining)
        {
            mining = false;
            CancelInvoke("MiningDelay");
            Invoke("MiningDelay", 1f);
            StopMining();
        }
        CancelInvoke("Mine");
    }

    void OnTriggerEnter(Collider other)
    {
        //Collider
        if (other.gameObject.tag == "Player")
        {
            //Collided With Player
            inOreTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopMining();
            inOreTrigger = false;
        }
    }

    void MiningDelay()
    {
        ableToMine = true;
    }
}
