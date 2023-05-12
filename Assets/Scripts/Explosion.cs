using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float damage = 50f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<BaseFunctions>().TakeDamage(damage);
        }

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
