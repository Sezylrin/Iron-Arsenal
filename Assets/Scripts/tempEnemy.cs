using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempEnemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
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
            Destroy(gameObject);
        }
    }
}
