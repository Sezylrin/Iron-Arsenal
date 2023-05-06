using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReaperBullet : EnemyBossProjectile
{
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
            Owner.Heal(Damage / 10);
            DeleteNow();
        }
    }
}
