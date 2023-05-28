using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AExplosive : AugmentBase
{
    // Start is called before the first frame update
    private float damage;

    public GameObject explosionPF;
    private GameObject spawnedExplosion;
    private Explosion explosionData;
    private float explosionScale = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (respawned == true && baseProjectile.modifiedPierce == 0 && baseProjectile.modifiedDamage != 0)
        {
            respawned = false;
            damage = baseProjectile.modifiedDamage;
            baseProjectile.modifiedDamage = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        if (baseProjectile.modifiedPierce == 0)
        {
            if (explosionPF && !spawnedExplosion)
            {
                spawnedExplosion = Instantiate(explosionPF, transform.position, Quaternion.identity);
                explosionData = spawnedExplosion.GetComponent<Explosion>();
                Vector3 newScale = spawnedExplosion.transform.localScale * baseProjectile.data.damageFactor * explosionScale;
                spawnedExplosion.transform.localScale =  newScale;
                explosionData.SetDamage(damage,true);
            }
        }
    }

    public void SetScale(float scale)
    {
        explosionScale = scale;
    }
}
