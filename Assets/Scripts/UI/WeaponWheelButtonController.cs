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
    private Image bgColor;
    private Color selectedColor = new Color(0f, 0.859f, 1f, 1f);
    private Color normalColor = new Color(1f, 1f, 1f, 0.647f);

    void Start()
    {
        anim = GetComponent<Animator>();
        bgColor = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
        if (WeaponWheelController.weaponID != id)
        {
            selected = false;
            bgColor.color = normalColor;
        }
    }

    public void Selected()
    {
        selected = true;
        gameObject.GetComponent<Image>().color = selectedColor;
        WeaponWheelController.weaponID = id;
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
