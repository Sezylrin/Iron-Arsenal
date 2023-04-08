using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Vector3 mouseLocation;
    private Vector3 worldPosition;

    private GameObject newCannonProjectile;
    private bool ableToShoot;
    private Transform projectilesParent;

    public Transform rotatePoint;
    public Transform cannonProjectileSpawnPoint;
    public LayerMask groundMask;
    

    public int activeCannonProjectile;                  // Determines which projectile  
    public GameObject[] cannonProjectileArray;          // to fire using following list
    private Pooling pooledBullets = new Pooling();      // 0 - Default (Bullets)
    private Pooling pooledShotguns = new Pooling();     // 1 - Shotgun
    private Pooling pooledRapidFires = new Pooling();   // 2 - Rapid Fire
    private Pooling pooledSlowShots = new Pooling();    // 3 - Slow Gun
    private Pooling pooledPoisonShots = new Pooling();  // 4 - Poison (DoT) Gun
    private Pooling pooledRockets = new Pooling();      // 5 - Rocket Launcher
    private Pooling pooledFlames = new Pooling();       // 6 - Flamethrower
    public List<Pooling> pools = new List<Pooling>();

    void Awake()
    {
        projectilesParent = GameObject.Find("Projectiles Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeCannonProjectile = 0;
        ableToShoot = true;

        pools.Add(pooledBullets);
        pools.Add(pooledShotguns);
        pools.Add(pooledRapidFires);
        pools.Add(pooledSlowShots);
        pools.Add(pooledPoisonShots);
        pools.Add(pooledRockets);
        pools.Add(pooledFlames);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.currentState == State.Building)
            return;
        //activeCannonProjectile = LevelManager.Instance.GetMode(); //Change this in the future
        mouseLocation = Input.mousePosition;
        mouseLocation.z += 1000000;

        worldPosition = MousePosition.MouseToWorld3D(Camera.main, groundMask);
        worldPosition.y = rotatePoint.transform.position.y;
        rotatePoint.LookAt(worldPosition, Vector3.down);

        if (Input.GetKey(KeyCode.Mouse0) && ableToShoot)
        {
            ableToShoot = false;

            if (pools[activeCannonProjectile].ListCount() > 0)
            {
                newCannonProjectile = pools[activeCannonProjectile].FirstObj();
                newCannonProjectile.SetActive(true);
                pools[activeCannonProjectile].RemoveObj(newCannonProjectile);
            }
            else
            {
                newCannonProjectile = Instantiate(cannonProjectileArray[activeCannonProjectile], projectilesParent);
            }
            newCannonProjectile.transform.rotation = transform.rotation;
            newCannonProjectile.transform.position = cannonProjectileSpawnPoint.position;

            ICannonProjectile cannonProjectileScript = newCannonProjectile.GetComponent<ICannonProjectile>();
            cannonProjectileScript.Direction = (mouseLocation - cannonProjectileSpawnPoint.position).normalized;
            cannonProjectileScript.Owner = this;
            cannonProjectileScript.Shoot();

            StartCoroutine(DelayFiring(cannonProjectileScript.FireDelay));
        }
    }

    IEnumerator DelayFiring(float delay)
    {
        yield return new WaitForSeconds(delay);
        ableToShoot = true;
    }

    public void PoolProjectile(GameObject obj, int projectileType)
    {
        obj.SetActive(false);
        pools[projectileType].AddObj(obj);
    }
}
