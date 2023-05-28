using System.Collections.Generic;
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
    private Button button;
    private Color selectedColor = new Color32(0, 219, 255, 255);
    private Color normalColor = new Color(1f, 1f, 1f, 0.647f);
    private static List<int> unlockedCannons;

    void Start()
    {
        unlockedCannons = new List<int>(LevelManager.Instance.playerFunctions.cannon.unlockedCannonProjectiles);
        anim = GetComponent<Animator>();
        bgColor = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
    }

    void Update()
    {
        if (!unlockedCannons.Contains(id))
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
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
        if (WeaponWheelController.weaponID == id && !selected)
        {
            Selected();
        }
    }

    public void Selected()
    {
        if (!button.interactable) return;
        selected = true;
        gameObject.GetComponent<Image>().color = selectedColor;
        WeaponWheelController.weaponID = id;
    }

    public void HoverEnter()
    {
        if (!button.interactable) return;
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        if (!button.interactable) return;
        anim.SetBool("Hover", false);
        itemText.text = "";
    }

    public static void UpdateUnlockedCannons()
    {
        unlockedCannons = new List<int>(LevelManager.Instance.playerFunctions.cannon.unlockedCannonProjectiles);
    }
}
