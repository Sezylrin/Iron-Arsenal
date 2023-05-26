using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASIceShards : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isInit = false;
    void Start()
    {
        Invoke("DestroySelf", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit)
            return;
        transform.Translate(Vector3.forward * 8 * Time.deltaTime);
    }

    public void Init(Vector3 dir)
    {
        transform.LookAt(dir + transform.position, Vector3.up);
        isInit = true;
    }
    
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(StatsManager.Instance.elementalDamage * 0.25f);
            other.gameObject.GetComponent<EnemyEffects>().IncreaseFreeze(5);
        }
    }
}
