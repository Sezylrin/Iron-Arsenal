using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASFireTrail : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject host;

    private bool trigger = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger)
        {
            trigger = true;
            return;
        }
        if (other.CompareTag("Enemy") && !other.gameObject.Equals(host))
        {
            other.GetComponent<Enemy>().TakeDamage(StatsManager.Instance.elementalDamage * 0.25f);
        }
    }
    public void Init(GameObject host)
    {
        this.host = host;
        Invoke("DestroySelf", 1f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
