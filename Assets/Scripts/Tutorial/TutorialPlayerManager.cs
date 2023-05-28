using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerManager : MonoBehaviour
{
    public BoxCollider col1;
    public BoxCollider col2;
    public BoxCollider col3;
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
        else if (other == col2)
        {
            TutorialManager.Instance.Collision4();
        }
        else if (other == col3)
        {
            TutorialManager.Instance.Sequence5();
        }
    }
}