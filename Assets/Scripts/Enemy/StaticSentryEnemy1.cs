using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSentryEnemy : Enemy
{
    public GameObject explosion;

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

    }

    protected override void OnDeath()
    {
        Instantiate(explosion, new Vector3(transform.position.x, -2, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }
}

