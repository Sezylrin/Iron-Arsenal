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
    private WeaponSounds weaponSounds;

    public GameObject projectilePF;
    private GameObject newCannonProjectile;
    public ParticleSystem flamethrower;
    private bool ableToShoot;
    private Transform projectilesParent;
    private bool mouseHeldDown = false;
    private bool firing = false;

    public ParticleSystem muzzleFlash;

    private Pooling pooledBullets = new Pooling();      // 0 - Default (Bullets)
    private Pooling pooledShotguns = new Pooling();     // 1 - Shotgun
    private Pooling pooledRapidFires = new Pooling();   // 2 - Rapid Fire
    private Pooling pooledSlowShots = new Pooling();    // 3 - Slow Gun
    private Pooling pooledPoisonShots = new Pooling();  // 4 - Poison (DoT) Gun
    private Pooling pooledRockets = new Pooling();      // 5 - Rocket Launcher
    private Pooling pooledFlames = new Pooling();       // 6 - Flamethrower
    public List<Pooling> pools = new List<Pooling>();

    public MeshRenderer[] meshes;

    void Awake()
    {
        projectilesParent = GameObject.Find("Projectiles Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponSounds = gameObject.GetComponent<WeaponSounds>();
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

        for (int i = 0; i < 7; i++)
        {
            unlockedCannonProjectiles.Add(-1);
        }

        lockedCannonProjectiles.Add(0);
        lockedCannonProjectiles.Add(1);
        lockedCannonProjectiles.Add(2);
        lockedCannonProjectiles.Add(3);
        lockedCannonProjectiles.Add(4);
        lockedCannonProjectiles.Add(5);
        lockedCannonProjectiles.Add(6);
        UnlockCannon(0);

        UpdateMaterial();

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

            if (activeCannonProjectile != flamethrowerIndex)
            {
                muzzleFlash.Play();
            }

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
            weaponSounds.ShootWeapon(activeCannonProjectile);
        }

        if (switchingEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && unlockedCannonProjectiles.Contains(0))
            {
                WeaponWheelController.weaponID = 0;
                SwitchCannon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedCannonProjectiles.Contains(1))
            {
                WeaponWheelController.weaponID = 1;
                SwitchCannon(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedCannonProjectiles.Contains(2))
            {
                WeaponWheelController.weaponID = 2;
                SwitchCannon(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && unlockedCannonProjectiles.Contains(3))
            {
                WeaponWheelController.weaponID = 3;
                SwitchCannon(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && unlockedCannonProjectiles.Contains(4))
            {
                WeaponWheelController.weaponID = 4;
                SwitchCannon(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) && unlockedCannonProjectiles.Contains(5))
            {
                WeaponWheelController.weaponID = 5;
                SwitchCannon(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) && unlockedCannonProjectiles.Contains(6))
            {
                WeaponWheelController.weaponID = 6;
                SwitchCannon(6);
            }
        }

    }

    public void SwitchCannon(int cannonType)
    {
        activeCannonProjectile = cannonType;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        CannonProjectile temp = cannonProjectileArray[unlockedCannonProjectiles[activeCannonProjectile]].GetComponent<CannonProjectile>();
        foreach (MeshRenderer mesh in meshes)
        {
            if (temp.cannonMat)
                mesh.material = temp.cannonMat;
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

    public void UnlockCannon(int cannonIndex)
    {
        if (lockedCannonProjectiles.Contains(cannonIndex))
        {
            unlockedCannonProjectiles[cannonIndex] = cannonIndex;
            lockedCannonProjectiles.Remove(cannonIndex);
            if (cannonIndex == 6)
            {
                flamethrowerIndex = 6;
            }
            UpdateMaterial();
        }
    }

    public void UnlockRandomCannon()
    {
        if (lockedCannonProjectiles.Count > 0)
        {
            int randomIndex = Random.Range(0, lockedCannonProjectiles.Count);
            int cannonToUnlock = lockedCannonProjectiles[randomIndex];

            UnlockCannon(cannonToUnlock);

            if (unlockedCannonProjectiles.Contains(cannonToUnlock))
            {
                activeCannonProjectile = cannonToUnlock;
                WeaponWheelController.weaponID = activeCannonProjectile;
                UpdateMaterial();
            }
        }
    }

    public void SetSwitchingEnabledState(bool state)
    {
        switchingEnabled = state;
    }
}
