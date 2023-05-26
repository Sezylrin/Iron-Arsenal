using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] Animator anim;
    [SerializeField] string itemName;
    [SerializeField] TMP_Text itemText;
    [SerializeField] private Image selectedItem;
    [SerializeField] private Sprite icon;

    private bool selected = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        Debug.Log("running selected");
        selected = true;
        WeaponWheelController.weaponID = id;
    }

    public void Deselected()
    {
        Debug.Log("running deselect");
        selected = false;
        WeaponWheelController.weaponID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}
