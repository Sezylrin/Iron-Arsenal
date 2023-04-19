using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public float currentHealth;
    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = CurrentHealth;
        SetRotation();
        Move();
    }
}
