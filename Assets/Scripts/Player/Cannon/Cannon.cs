using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Vector3 mouseLocation;
    private Vector3 worldPosition;

    [field: Header("Cannon")]
    public int activeCannonProjectile;
    public GameObject[] cannonProjectileArray;

    [field: Header("Flamethrower")]
    public CannonProjectileData flameData;

    [field: Header("Other")]
    public Transform rotatePoint;
    public Transform cannonProjectileSpawnPoint;
    public LayerMask groundMask;

    private GameObject newCannonProjectile;
    private ParticleSystem flamethrower;
    private bool ableToShoot;
    private Transform projectilesParent;
    private bool mouseHeldDown = false;
    private bool firing = false;
        
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
        flamethrower = gameObject.GetComponent<ParticleSystem>();
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

        var main = flamethrower.main;
        main.startSpeed = 9.5f + (flameData.projectileSpeed * flameData.level / 2);

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            mouseHeldDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouseHeldDown = false;
        }

        if (activeCannonProjectile == 6 && mouseHeldDown && !firing) 
        {
            firing = true;
            flamethrower.Play();
        }

        if ((activeCannonProjectile == 6 && !mouseHeldDown && firing) || activeCannonProjectile != 6)
        {
            firing = false;
            flamethrower.Stop();
        }

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

            CannonProjectile cannonProjectileScript = newCannonProjectile.GetComponent<CannonProjectile>();
            cannonProjectileScript.Direction = (mouseLocation - cannonProjectileSpawnPoint.position).normalized;
            cannonProjectileScript.Owner = this;
            cannonProjectileScript.SetStats();
            cannonProjectileScript.Shoot();

            StartCoroutine(DelayFiring(cannonProjectileScript.FireDelay));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeCannonProjectile = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeCannonProjectile = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeCannonProjectile = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeCannonProjectile = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activeCannonProjectile = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            activeCannonProjectile = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            activeCannonProjectile = 6;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.Bullet);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.Shotgun);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.RapidFire);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.SlowShot);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.PoisonShot);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.Rocket);
            DataManager.instance.UpgradeCannonProjectile(CannonProjectileType.Flame);
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
