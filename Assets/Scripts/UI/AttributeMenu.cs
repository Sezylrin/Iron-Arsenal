using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AttributeMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] attributeButtons;

    [Header("Attribute Info")]
    [SerializeField] private AttributeSlot[] attributeSlots;
    [SerializeField] private AttributeUpgradeData[] attributeUpgrades = new AttributeUpgradeData[3];

    private void Start()
    {
        for (int i = 0; i < attributeButtons.Length; i++)
        {
            int augmentIndex = i;
            attributeButtons[i].onClick.AddListener(() => SelectAugment(augmentIndex));
        }
    }

    public void SetAttributes()
    {
        for (int i = 0; i < attributeSlots.Length; i++)
        {
            attributeSlots[i].SetAttributeData(attributeUpgrades[i]);
        }
    }

    public void SelectAugment(int attributeIndex)
    {
        switch (attributeUpgrades[attributeIndex].attribute)
        {
            case Attribute.Physical:
                StatsManager.Instance.UpgradeDamage();
                break;
            case Attribute.Health:
                StatsManager.Instance.UpgradeHealth();
                break;
            case Attribute.Elemental:
                StatsManager.Instance.UpgradeElemental();
                break;
        }
        LevelCanvasManager.Instance.RemoveAttributeChoices();
    }
}

[System.Serializable]
public class AttributeSlot
{
    [SerializeField] private Image attributeImage;
    [SerializeField] private TMP_Text attributeName;
    [SerializeField] private TMP_Text attributeDesc;

    public void SetAttributeData(AttributeUpgradeData attribute)
    {
        attributeImage.sprite = attribute.icon;
        attributeName.text = attribute.upgradeName;
        string descText = attribute.description;
        switch (attribute.attribute) {
            case Attribute.Physical:
                attributeDesc.text = descText + " " + ((int)(StatsManager.Instance.PhysicalUpgradeAmount() * 100)).ToString() + "%"; ;
                break;
            case Attribute.Health:
                attributeDesc.text = descText + " " + ((int)(StatsManager.Instance.HealthUpgradeAmount() * 100)).ToString() + "%";
                break;
            case Attribute.Elemental:
                attributeDesc.text = descText + " " + ((int)(StatsManager.Instance.ElementalUpgradeAmount() * 100)).ToString() + "%";
                break;
        }
    }
}