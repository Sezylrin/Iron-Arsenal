using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public enum OreType
    {
        Xenorium,
        Novacite,
        Voidstone
    };

    public OreType type;

    private Mining miningScript; // Temp
    private LevelManager levelManager;
    private AudioSource audioClip;

    public int totalResourcesInDeposit;
    public int currentResourcesInDeposit;

    public bool inOreTrigger;
    public bool ableToMine;
    public bool mining;
    public bool ableToStopMining;

    private Coroutine miningCoroutine;
    void Awake()
    {
        miningScript = GameObject.Find("Player").GetComponent<Mining>();
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
        ableToStopMining = true;
        levelManager = LevelManager.Instance;
        transform.localScale = new Vector3(5 + totalResourcesInDeposit/100, 0.1f, 5 + totalResourcesInDeposit/100);
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
        if (inOreTrigger)
        {
            if (miningScript.playerRB.velocity.magnitude > 0.1f)
                StopMining();
        }
    }

    public IEnumerator Mine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            audioClip.Play();

            if (type == OreType.Xenorium)
            {
                if (currentResourcesInDeposit - miningScript.miningOutput <= 0)
                {
                    levelManager.GainIron(currentResourcesInDeposit);
                    Destroy(gameObject);
                }
                else
                {
                    currentResourcesInDeposit -= miningScript.miningOutput;
                    levelManager.GainIron(miningScript.miningOutput);
                }
            }
            else if (type == OreType.Novacite)
            {
                if (currentResourcesInDeposit - miningScript.miningOutput <= 0)
                {
                    levelManager.GainCopper(currentResourcesInDeposit);
                    Destroy(gameObject);
                }
                else
                {
                    currentResourcesInDeposit -= miningScript.miningOutput;
                    levelManager.GainCopper(miningScript.miningOutput);
                }
            }
            else if (type == OreType.Voidstone)
            {
                if (currentResourcesInDeposit - miningScript.miningOutput <= 0)
                {
                    levelManager.GainGold(currentResourcesInDeposit);
                    Destroy(gameObject);
                }
                else
                {
                    currentResourcesInDeposit -= miningScript.miningOutput;
                    levelManager.GainGold(miningScript.miningOutput);
                }
            }
        }
    }

    public void StartMining()
    {
        miningCoroutine = StartCoroutine(Mine(miningScript.miningSpeed));
        ableToStopMining = false;
        StartCoroutine(StopMiningDelay());
    }

    public void StopMining()
    {
        if (ableToStopMining)
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
    }

    IEnumerator MiningDelay()
    {
        yield return new WaitForSeconds(1f);
        ableToMine = true;
    }

    IEnumerator StopMiningDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ableToStopMining = true;
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
