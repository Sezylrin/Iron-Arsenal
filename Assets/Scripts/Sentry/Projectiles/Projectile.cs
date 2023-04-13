using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public ProjectileData data;
    
    public Vector3 dir;

    public float modifiedDamage;

    public float modifiedSpeed;

    public int modifiedPierce;

    private float timer = 0;

    private Sentry owner;

    public List<Augments> activeAugments;

    public bool respawned;
    void Start()
    {
        timer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!data)
            return;
        TranslateDir();
        if(timer <= 0)
        {
            timer = 3;
            owner.PoolBullet(gameObject);
            gameObject.SetActive(false);
        }
        timer -= Time.deltaTime;
    }

    public void TranslateDir()
    {
        transform.Translate(dir * modifiedSpeed * Time.deltaTime);
    }
    public void SetProjectileStat()
    {
        modifiedDamage = data.baseDamage;
        modifiedSpeed = data.bulletSpeed;
        modifiedPierce = data.pierce;
    }

    public void SetDirection(Vector3 dir)
    {
        this.dir = dir.normalized;
    }

    public void setSpawn(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetProjectileData(ProjectileData data, Sentry owner)
    {
        if (!this.data)
            this.data = data;
        if (!this.owner)
            this.owner = owner;
        SetProjectileStat();
    }

    

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("running");
        if (other.gameObject.tag == "Enemy")
        {
            //other.gameObject.GetComponent<Enemy>().TakeDamage(modifiedDamage);
            if (modifiedPierce <= 0)
            {
                owner.PoolBullet(gameObject);
                gameObject.SetActive(false);
            }
            else
                modifiedPierce--;
        }
    }

}
