using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    [SerializeField] private bool debugSpawnWeapons = false;
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite pistolImg;
    public static int weaponID;

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
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }
        cannon.SwitchCannon(weaponID);
    }
}
