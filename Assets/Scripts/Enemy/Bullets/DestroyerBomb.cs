using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class DestroyerBomb : EnemyBossProjectile
{
    public GameObject explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LevelManager.Instance.playerFunctions.TakeDamage(Damage);
            GameObject explosionObj = Instantiate(explosion, transform.position, transform.rotation);
            explosionObj.GetComponent<Explosion>().SetDamage(Damage);
            DeleteNow();
        }
    }
}
