using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseItem : MonoBehaviour
{
    [SerializeField] private Image iconBorder;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text xenoriumCost;
    [SerializeField] private TMP_Text novaciteCost;
    [SerializeField] private TMP_Text voidStoneCost;

    private MechUpgradeData mechUpgradeData;
    private AttributeUpgradeData attributeUpgradeData;
    private AugmentData augmentData;
    private SentryData sentryData;
    private ShopMenu shopMenu;
    public TabType currTab;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HandlePurchase);
    }
    public void SetMechUpgrade(MechUpgradeData mechUpgradeData, TabType currTab, ShopMenu shopMenu)
    {
        GreyOutIfCantPurchase(mechUpgradeData.xenoriumCost, mechUpgradeData.novaciteCost, mechUpgradeData.voidStoneCost);
        icon.sprite = mechUpgradeData.icon;
        title.text = mechUpgradeData.upgradeName;
        description.text = mechUpgradeData.description;
        xenoriumCost.text = mechUpgradeData.xenoriumCost.ToString();
        novaciteCost.text = mechUpgradeData.novaciteCost.ToString();
        voidStoneCost.text = mechUpgradeData.voidStoneCost.ToString();
        this.mechUpgradeData = mechUpgradeData;
        this.currTab = currTab;
        this.shopMenu = shopMenu;
    }

    public void SetAttributeUpgrade(AttributeUpgradeData attributeUpgradeData, TabType currTab, ShopMenu shopMenu)
    {
        GreyOutIfCantPurchase(attributeUpgradeData.xenoriumCost, attributeUpgradeData.novaciteCost, attributeUpgradeData.voidStoneCost);
        icon.sprite = attributeUpgradeData.icon;
        title.text = attributeUpgradeData.upgradeName;

        string descTxt = attributeUpgradeData.description;
        if (attributeUpgradeData.attribute == Attribute.Physical)
        {
            descTxt += " " + ((int)(StatsManager.Instance.PhysicalUpgradeAmount()*100)).ToString() + "%";
        }
        else if (attributeUpgradeData.attribute == Attribute.Health)
        {
            descTxt += " " + ((int)(StatsManager.Instance.HealthUpgradeAmount() * 100)).ToString() + "%";
        }
        else if (attributeUpgradeData.attribute == Attribute.Elemental)
        {
            descTxt += " " + ((int)(StatsManager.Instance.ElementalUpgradeAmount() * 100)).ToString() + "%";
        }

        description.text = descTxt;
        xenoriumCost.text = attributeUpgradeData.xenoriumCost.ToString();
        novaciteCost.text = attributeUpgradeData.novaciteCost.ToString();
        voidStoneCost.text = attributeUpgradeData.voidStoneCost.ToString();
        this.attributeUpgradeData = attributeUpgradeData;
        this.currTab = currTab;
        this.shopMenu = shopMenu;
    }

    public void SetAugmentPurchase(AugmentData augmentData, TabType currTab, ShopMenu shopMenu )
    {
        GreyOutIfCantPurchase(augmentData.xenoriumCost, augmentData.novaciteCost, augmentData.voidStoneCost);
        icon.sprite = augmentData.icon;
        title.text = augmentData.augName;
        description.text = augmentData.description;
        xenoriumCost.text = augmentData.xenoriumCost.ToString();
        novaciteCost.text = augmentData.novaciteCost.ToString();
        voidStoneCost.text = augmentData.voidStoneCost.ToString();
        this.augmentData = augmentData;
        this.currTab = currTab;
        this.shopMenu = shopMenu;
    }

    public void SetSentryPurchase(SentryData sentryData, TabType currTab, ShopMenu shopMenu) {
        GreyOutIfCantPurchase(sentryData.unlockXenoriumCost, sentryData.unlockNovaciteCost, sentryData.unlockVoidStoneCost);
        icon.sprite = sentryData.SentryIcon;
        title.text = sentryData.sentryName;
        description.text = sentryData.description;
        xenoriumCost.text = sentryData.unlockXenoriumCost.ToString();
        novaciteCost.text = sentryData.unlockNovaciteCost.ToString();
        voidStoneCost.text = sentryData.unlockVoidStoneCost.ToString();
        this.sentryData = sentryData;
        this.currTab = currTab;
        this.shopMenu = shopMenu;
    }

    public void GreyOutIfCantPurchase(int xenoriumCost, int novaciteCost, int voidStoneCost)
    {
        Color colourToSet = new Color32(0x80, 0x80, 0x80, 0xFF);
        Color titleColour = new Color32(0x80, 0x80, 0x80, 0xFF);
        Color32 descColour = new Color32(0x80, 0x80, 0x80, 0xFF);
        if (LevelManager.Instance.CanPurchaseItem(xenoriumCost, novaciteCost, voidStoneCost))
        {
            colourToSet = Color.white;
            titleColour = new Color32(0xFF, 0xCB, 0x5C, 0xFF);
            descColour = new Color32(0x80, 0xD9, 0xFF, 0xFF);
        }
        gameObject.GetComponent<Image>().color = colourToSet;
        iconBorder.color = colourToSet;
        icon.color = colourToSet;
        title.color = titleColour;
        description.color = descColour;
    }

    private void HandlePurchase()
    {
        switch (currTab)
        {
            case TabType.mechUpgrades:
                if (LevelManager.Instance.PurchaseItemIfPossible(mechUpgradeData.xenoriumCost, mechUpgradeData.novaciteCost, mechUpgradeData.voidStoneCost))
                {
                    switch(mechUpgradeData.upgradeType)
                    {
                        case UpgradeType.health:
                            LevelManager.Instance.playerFunctions.ShopRecoverHealth();
                            break;
                        case UpgradeType.drill:
                            LevelManager.Instance.playerMining.UpgradeMining();
                            break;
                        case UpgradeType.baseSize:
                            LevelManager.Instance.playerFunctions.UpgradeBase();
                            break;
                    }
                    shopMenu.PurchaseItem();
                }
                break;
            case TabType.attributeUpgrades:
                if (LevelManager.Instance.PurchaseItemIfPossible(attributeUpgradeData.xenoriumCost, attributeUpgradeData.novaciteCost, attributeUpgradeData.voidStoneCost))
                {
                    switch (attributeUpgradeData.attribute) {
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
                    shopMenu.PurchaseItem();
                }
                break;
            case TabType.augmentPurchases:
                if (LevelManager.Instance.PurchaseItemIfPossible(augmentData.xenoriumCost, augmentData.novaciteCost, augmentData.voidStoneCost))
                {
                    AugmentManager.Instance.AddAugment(augmentData.augmentType);
                    shopMenu.PurchaseAugment(augmentData);
                }
                break;
            case TabType.sentryPurchases:
                if (LevelManager.Instance.PurchaseItemIfPossible(sentryData.unlockXenoriumCost, sentryData.unlockNovaciteCost, sentryData.unlockVoidStoneCost))
                {
                    SentryManager.Instance.AddSentry(sentryData);
                    shopMenu.PurchaseSentry(sentryData);
                }
                break;
        }
    }
}
