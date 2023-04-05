using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayer : MonoBehaviour
{
    public float speed;
    public GameObject ore;

    public float miningSpeed;
    public int miningOutput;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.05f;

        miningSpeed = 1f; 
        miningOutput = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed);
            if (ore)
            {
                ore.GetComponent<Ore>().StopMining();
            } 
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed);
            if (ore)
            {
                ore.GetComponent<Ore>().StopMining();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed);
            if (ore)
            {
                ore.GetComponent<Ore>().StopMining();
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed);
            if (ore)
            {
                ore.GetComponent<Ore>().StopMining();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ore")
        {
            ore = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ore")
        {
            ore.GetComponent<Ore>().StopMining();
            ore = null;
        }
    }
}
