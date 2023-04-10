using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : Enemy
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
        SetRotation();
        Move();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            TakeDamage(col.gameObject.GetComponent<tempPlayer>().ramDamage);
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        Instantiate(explosion, transform.position, transform.rotation);

        base.OnDeath();
    }
}
