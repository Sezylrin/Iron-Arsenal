using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEnemyManagerAtRuntime : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Enemy>().Manager = EnemyManager.Instance;
    }
}