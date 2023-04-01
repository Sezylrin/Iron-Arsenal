using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Transform rotatePoint;
    private Vector3 mouseLocation;
    private Vector3 worldPosition;
    private Transform bulletSpawnPoint;
    public GameObject[] bulletArray;
    private GameObject newBullet;

    void Awake()
    {
        rotatePoint = GameObject.Find("Gun Rotation Point").transform;
        bulletSpawnPoint = GameObject.Find("Bullet Spawn Point").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        mouseLocation = Input.mousePosition;
        mouseLocation.z += 1000000;

        worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        worldPosition.y = 0;

        rotatePoint.LookAt(worldPosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            newBullet = Instantiate(bulletArray[0], bulletSpawnPoint.position, transform.rotation);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.direction = (mouseLocation - bulletSpawnPoint.position).normalized;
        }
    }
}
