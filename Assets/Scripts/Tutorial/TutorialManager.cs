using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Enemy>().Manager = EnemyManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}