using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public ProjectileData data;
    
    public Vector3 dir;

    public float baseDamage;

    public float speed;

    public int pierce;

    private float timer = 0;

    private Sentry owner;
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
        transform.Translate(dir * speed * Time.deltaTime);
    }
    public void SetProjectileStat()
    {
        baseDamage = data.baseDamage;
        speed = data.bulletSpeed;
        pierce = data.pierce;
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
        this.data = data;
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
