using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerShield : MonoBehaviour
{
    [field: SerializeField] public Destroyer DestroyerScript { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            BaseFunctions tempBase = col.gameObject.GetComponent<BaseFunctions>();
            
            DestroyerScript.EnemyRB.AddForce(Vector3.Normalize(new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z)) * DestroyerScript.RamLaunchMultiplier, ForceMode.Impulse);
        }
    }
}
