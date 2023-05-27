using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerManager : MonoBehaviour
{
    public BoxCollider col1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == col1)
        {
            Destroy(other.gameObject);
            TutorialManager.Instance.Collision1();
        }
    }
}