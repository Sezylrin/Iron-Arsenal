using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : CannonProjectile
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
        Move();
    }

    public override IEnumerator OnDelete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosion, new Vector3(transform.position.x, -2, transform.position.z), transform.rotation);
        base.DeleteNow();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            DeleteNow();
        }
    }
}
