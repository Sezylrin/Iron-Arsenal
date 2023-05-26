using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite pistolImg;
    public static int weaponID;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(LevelManager.Instance.playerFunctions.cannon.switchingEnabled);
            LevelManager.Instance.playerFunctions.cannon.SetSwitchingEnabledState(true);
            LevelManager.Instance.playerFunctions.cannon.UnlockRandomCannon();
            WeaponWheelButtonController.UpdateUnlockedCannons();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && LevelManager.Instance.playerFunctions.cannon.switchingEnabled)
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

        switch (weaponID)
        {
            case 0:
                //Pistol
                break;
            case 1:
                //Shotgun
                break;
            case 2:
                //Rapid fire
                break;
            case 3:
                //Slow Gun
                break;
            case 4:
                //Poison
                break;
            case 5:
                //Rocket Launcher
                break;
            case 6:
                //Flamethrower
                break;
            case 7:
                //Sniper
                break;
        }
    }
}
