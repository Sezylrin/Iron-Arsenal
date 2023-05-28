using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawRay(transform.position, transform.forward*100,Color.green,float.MaxValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
