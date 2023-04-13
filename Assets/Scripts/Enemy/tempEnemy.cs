using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempEnemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public TempEnemyManager manager;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        manager = GameObject.Find("EnemyManager").GetComponent<TempEnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    public void takeDamage(float x)
    {
        currentHealth -= x;
        if (currentHealth <= 0 ) 
        {
            manager.enemyList.Remove(transform);
            Destroy(gameObject);
        }
    }
}
