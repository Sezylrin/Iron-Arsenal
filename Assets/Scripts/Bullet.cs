using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICannonProjectile
{
    private Vector3 direction;
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private float damage = 10f;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private float speed = 0.1f;
    public float ProjectileSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    private float fireDelay = 0.3f;
    public float FireDelay
    {
        get { return fireDelay; }
        set { fireDelay = value; }
    }

    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider.enabled = false;
        Invoke("enableCollider", 0.1f);
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

    void enableCollider()
    {
        boxCollider.enabled = true;
    }
}
