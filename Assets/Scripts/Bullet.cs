using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
        Invoke("delete", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.x * speed, 0, direction.z * speed);
    }

    void delete()
    {
        Destroy(gameObject);
    }
}
