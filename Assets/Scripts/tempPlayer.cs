using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayer : MonoBehaviour
{
    public float speed;
    public GameObject ore;
    public bool inOreTrigger;
    public bool ableToMine;
    public bool mining;
    public float miningSpeed;
    public int miningOutput;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.05f;
        inOreTrigger = false;
        ableToMine = true;

        miningSpeed = 1f; 
        miningOutput = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed);
            stopMining();
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed);
            stopMining();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed);
            stopMining();
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed);
            stopMining();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (inOreTrigger && ore && ableToMine && !mining)
            {
                mining = true;
                ableToMine = false;
                ore.GetComponent<Ore>().startMining();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ore")
        {
            inOreTrigger = true;
            ore = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ore")
        {
            stopMining();
            ore = null;
        }
    }

    void stopMining()
    {
        if (mining)
        {
            mining = false;
            CancelInvoke("miningDelay");
            Invoke("miningDelay", 1f);
            if (ore)
            {
                ore.GetComponent<Ore>().stopMining();
            }     
        }
    }

    void miningDelay()
    {
        ableToMine = true;
    }
}
