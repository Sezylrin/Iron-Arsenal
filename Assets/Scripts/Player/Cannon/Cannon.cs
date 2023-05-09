using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Vector3 mouseLocation;
    private Vector3 worldPosition;

    [field: Header("Cannon")]
    public int activeCannonProjectile;
    public GameObject[] cannonProjectileArray;
    public List<int> lockedCannonProjectiles;
    public List<int> unlockedCannonProjectiles;
    public bool switchingEnabled;

    [field: Header("Flamethrower")]
    public CannonProjectileData flameData;
    public int flamethrowerIndex;

    [field: Header("Other")]
    public Transform rotatePoint;
    public Transform cannonProjectileSpawnPoint;
    public LayerMask groundMask;

    public GameObject projectilePF;
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
        flamethrowerIndex = -1;
        ableToShoot = true;
        switchingEnabled = true;

        pools.Add(pooledBullets);
        pools.Add(pooledShotguns);
        pools.Add(pooledRapidFires);
        pools.Add(pooledSlowShots);
        pools.Add(pooledPoisonShots);
        pools.Add(pooledRockets);
        pools.Add(pooledFlames);

        unlockedCannonProjectiles.Add(0);
        lockedCannonProjectiles.Add(1);
        lockedCannonProjectiles.Add(2);
        lockedCannonProjectiles.Add(3);
        lockedCannonProjectiles.Add(4);
        lockedCannonProjectiles.Add(5);
        lockedCannonProjectiles.Add(6);
                
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

        

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            mouseHeldDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouseHeldDown = false;
        }

        if (activeCannonProjectile == flamethrowerIndex && mouseHeldDown && !firing) 
        {
            firing = true;
            flamethrower.Play();
        }

        if ((activeCannonProjectile == flamethrowerIndex && !mouseHeldDown && firing) || activeCannonProjectile != flamethrowerIndex)
        {
            firing = false;
            flamethrower.Stop();
        }

        if (Input.GetKey(KeyCode.Mouse0) && ableToShoot)
        {
            ableToShoot = false;

            if (pools[unlockedCannonProjectiles[activeCannonProjectile]].ListCount() > 0)
            {
                newCannonProjectile = pools[unlockedCannonProjectiles[activeCannonProjectile]].FirstObj();
                newCannonProjectile.SetActive(true);
                pools[unlockedCannonProjectiles[activeCannonProjectile]].RemoveObj(newCannonProjectile);
            }
            else
            {
                newCannonProjectile = Instantiate(cannonProjectileArray[unlockedCannonProjectiles[activeCannonProjectile]], projectilesParent);
            }
            newCannonProjectile.transform.rotation = transform.rotation;
            newCannonProjectile.transform.position = cannonProjectileSpawnPoint.position;

            CannonProjectile cannonProjectileScript = newCannonProjectile.GetComponent<CannonProjectile>();
            cannonProjectileScript.SetStats(this, worldPosition);
            if (cannonProjectileScript.activeAugments.Count <= AugmentManager.Instance.activeAugments.Count)
            {
                foreach (Augments augment in AugmentManager.Instance.activeAugments)
                {
                    cannonProjectileScript.AddAugments(augment);
                }
            }
            cannonProjectileScript.Init();
        }

        if (switchingEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                activeCannonProjectile = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (unlockedCannonProjectiles.Count >= 2)
                {
                    activeCannonProjectile = 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (unlockedCannonProjectiles.Count >= 3)
                {
                    activeCannonProjectile = 2;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (unlockedCannonProjectiles.Count >= 4)
                {
                    activeCannonProjectile = 3;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (unlockedCannonProjectiles.Count >= 5)
                {
                    activeCannonProjectile = 4;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (unlockedCannonProjectiles.Count >= 6)
                {
                    activeCannonProjectile = 5;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (unlockedCannonProjectiles.Count >= 7)
                {
                    activeCannonProjectile = 6;
                }
            }
        }
        

        if (Input.GetKeyDown(KeyCode.M))
        {
            UnlockRandomCannon();
        }
    }

    public void DelayFiring()
    {
        ableToShoot = true;
    }


    public void PoolProjectile(GameObject obj, int projectileType)
    {
        obj.SetActive(false);
        pools[projectileType].AddObj(obj);
    }
    
    public void UnlockRandomCannon()
    {
        if (lockedCannonProjectiles.Count > 0)
        {
            int random = Random.Range(0, lockedCannonProjectiles.Count);
            unlockedCannonProjectiles.Add(lockedCannonProjectiles[random]);
            lockedCannonProjectiles.RemoveAt(random);

            if (unlockedCannonProjectiles[unlockedCannonProjectiles.Count - 1] == 6)
            {
                flamethrowerIndex = unlockedCannonProjectiles.Count - 1;
            }

            activeCannonProjectile = unlockedCannonProjectiles.Count - 1;
        }
    }

    public void SetSwitchingEnabledState(bool state)
    {
        switchingEnabled = state;
    }
}
