using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController Instance { get; private set; }
    [SerializeField] private bool debugSpawnWeapons = false;
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite pistolImg;
    public static int weaponID;
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update()
    {
        Cannon cannon = LevelManager.Instance.playerFunctions.cannon;

        if (Input.GetKeyDown(KeyCode.H) && debugSpawnWeapons)
        {
            cannon.SetSwitchingEnabledState(true);
            cannon.UnlockRandomCannon();
            WeaponWheelButtonController.UpdateUnlockedCannons();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && cannon.switchingEnabled)
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
            if (GameManager.Instance.currentSelection != CurrentSelection.Paused)
                GameManager.Instance.PauseGame();
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
            if (GameManager.Instance.currentSelection != CurrentSelection.Playing)
                GameManager.Instance.ResumeGame();
        }
        cannon.SwitchCannon(weaponID);
    }

    public void closeMenu()
    {
        weaponWheelSelected = false;
    }
}
