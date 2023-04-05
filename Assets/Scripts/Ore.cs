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

    private tempPlayer playerScript; // Temp
    private LevelManager levelManager;
    private AudioSource audioClip;

    public int totalResourcesInDeposit;
    public int currentResourcesInDeposit;

    public bool inOreTrigger;
    public bool ableToMine;
    public bool mining;

    private Coroutine miningCoroutine;
    void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<tempPlayer>();
        audioClip = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalResourcesInDeposit = Random.Range(100, 501);
        currentResourcesInDeposit = totalResourcesInDeposit;
        inOreTrigger = false;
        ableToMine = true;
        mining = false;
        levelManager = LevelManager.Instance;
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

    public IEnumerator Mine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            audioClip.Play();

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
    }

    public void StartMining()
    {
        miningCoroutine = StartCoroutine(Mine(playerScript.miningSpeed));
    }

    public void StopMining()
    {
        if (miningCoroutine != null)
        {
            StopCoroutine(miningCoroutine);
        }
  
        if (mining)
        {
            mining = false;
            StopCoroutine(MiningDelay());
            StartCoroutine(MiningDelay());
        }
    }

    IEnumerator MiningDelay()
    {
        yield return new WaitForSeconds(1f);
        ableToMine = true;
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
}
