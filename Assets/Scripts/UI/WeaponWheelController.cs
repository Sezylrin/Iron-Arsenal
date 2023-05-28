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
        Cannon cannon = LevelManager.Instance.playerFunctions.cannon;
        if (Input.GetKeyDown(KeyCode.H))
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

        //switch (weaponID)
        //{
            //case 0:
                //cannon.SwitchCannon(weaponID);
                //break;
            //case 1:
                //Shotgun
                //break;
           // case 2:
                //Rapid fire
                //break;
            //case 3:
                //Slow Gun
                //break;
            //case 4:
                //Poison
                //break;
            //case 5:
                //Rocket Launcher
                //break;
            //case 6:
                //Flamethrower
                //break;
            //case 7:
                //Sniper
                //break;
        //}
    }
}
