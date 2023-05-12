using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayer : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float ramDamage;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 250;
        currentHealth = maxHealth;
        ramDamage = 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {

    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            TakeDamage(col.gameObject.GetComponent<Enemy>().DamageOnCollide);
        }
        if (col.gameObject.tag == "Wall")
        {
            TakeDamage(col.gameObject.GetComponent<DestroyerShield>().DestroyerScript.DamageOnCollide);
        }
    }
}
