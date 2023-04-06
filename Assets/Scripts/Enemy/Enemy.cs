using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public EnemyManager manager;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
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
