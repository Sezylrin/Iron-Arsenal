using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASShieldExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> enemies = new List<GameObject>();
    public float maxRadius;
    public float expansionTime;
    private float expansionRate;
    private SphereCollider sphere;
    public float force;
    void Start()
    {
        expansionRate = maxRadius / expansionTime;
        sphere = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float scaleRate = expansionRate * Time.deltaTime;
        Vector3 scale = new Vector3(scaleRate, scaleRate, scaleRate);
        if (transform.localScale.magnitude < maxRadius)
            transform.localScale += scale;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemies.Contains(other.gameObject))
        {
            enemies.Add(other.gameObject);
            Vector3 dir = other.transform.position - transform.position;
            dir.y = 0;
            other.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
            other.GetComponent<Enemy>().TakeDamage(StatsManager.Instance.healthFactor * 0.2f);
        }
    }
}
