using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemyAtRuntime : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Enemy>().Manager = EnemyManager.Instance;
        gameObject.GetComponent<Enemy>().Speed = 0;
    }
}