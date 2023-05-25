using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] obj;
    void Start()
    {
        foreach (GameObject child in obj)
        {
            child.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
