using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Transform rotatePoint;
    private Vector3 mouseLocation;
    private Vector3 worldPosition;
    private GameObject newCannonProjectile;
    private Transform cannonProjectileSpawnPoint;
    public GameObject[] cannonProjectileArray;
    public int activeCannonProjectile;

    void Awake()
    {
        rotatePoint = GameObject.Find("Cannon Rotation Point").transform;
        cannonProjectileSpawnPoint = GameObject.Find("Cannon Projectile Spawn Point").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeCannonProjectile = 0;
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
            newCannonProjectile = Instantiate(cannonProjectileArray[activeCannonProjectile], cannonProjectileSpawnPoint.position, transform.rotation);
            ICannonProjectile cannonProjectileScript = newCannonProjectile.GetComponent<ICannonProjectile>();
            cannonProjectileScript.Direction = (mouseLocation - cannonProjectileSpawnPoint.position).normalized;
        }
    }
}
