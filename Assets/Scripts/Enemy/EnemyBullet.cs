using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public EnemyCannon Owner { get; set; }
    public Vector3 Direction { get; set; }
    public float Damage { get; set; } = 10f;
    public float ProjectileSpeed { get; set; } = 10f;
    public float FireDelay { get; set; } = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction.x * ProjectileSpeed * Time.deltaTime, 0, Direction.z * ProjectileSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        StopCoroutine(Delete(0f));
        StartCoroutine(Delete(2f));
    }

    public IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.enemyScript.Manager.PoolEnemyBullet(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<tempPlayer>().TakeDamage(Damage);
            StartCoroutine(Delete(0f));
        }
    }
}
