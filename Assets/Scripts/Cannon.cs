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
    public int activeCannonProjectile;
    public GameObject[] cannonProjectileArray;
    public Transform projectilesParent;
    private bool ableToShoot;

    void Awake()
    {
        rotatePoint = GameObject.Find("Cannon Rotation Point").transform;
        cannonProjectileSpawnPoint = GameObject.Find("Cannon Projectile Spawn Point").transform;
        projectilesParent = GameObject.Find("Projectiles").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeCannonProjectile = 0;
        ableToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        mouseLocation = Input.mousePosition;
        mouseLocation.z += 1000000;

        worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        worldPosition.y = 0;

        rotatePoint.LookAt(worldPosition);

        if (Input.GetKey(KeyCode.Mouse0) && ableToShoot)
        {
            ableToShoot = false;
            newCannonProjectile = Instantiate(cannonProjectileArray[activeCannonProjectile], cannonProjectileSpawnPoint.position, transform.rotation, projectilesParent);
            ICannonProjectile cannonProjectileScript = newCannonProjectile.GetComponent<ICannonProjectile>();
            cannonProjectileScript.Direction = (mouseLocation - cannonProjectileSpawnPoint.position).normalized;
            Invoke("delayFiring", cannonProjectileScript.FireDelay);
        }
    }

    void delayFiring()
    {
        ableToShoot = true;
    }
}
