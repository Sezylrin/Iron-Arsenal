using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text xenoriumCost;
    [SerializeField] private TMP_Text novaciteCost;
    [SerializeField] private TMP_Text voidStoneCost;

    private MechUpgradeData mechUpgradeData;
    private AttributeUpgradeData attributeUpgradeData;
    private SentryData sentryData;
    private ShopMenu shopMenu;
    public TabType currTab;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HandlePurchase);
    }
    public void SetMechUpgrade(MechUpgradeData mechUpgradeData, TabType currTab, ShopMenu shopMenu)
    {
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
        icon.sprite = attributeUpgradeData.icon;
        title.text = attributeUpgradeData.upgradeName;
        description.text = attributeUpgradeData.description;
        xenoriumCost.text = attributeUpgradeData.xenoriumCost.ToString();
        novaciteCost.text = attributeUpgradeData.novaciteCost.ToString();
        voidStoneCost.text = attributeUpgradeData.voidStoneCost.ToString();
        this.attributeUpgradeData = attributeUpgradeData;
        this.currTab = currTab;
        this.shopMenu = shopMenu;
    }

    public void SetSentryPurchase(SentryData sentryData, TabType currTab, ShopMenu shopMenu) {
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

    private void HandlePurchase()
    {
        switch (currTab)
        {
            case TabType.mechUpgrades:
                if (LevelManager.Instance.PurchaseItemIfPossible(mechUpgradeData.xenoriumCost, mechUpgradeData.novaciteCost, mechUpgradeData.voidStoneCost))
                {
                    //TODO: Handle purchasing mechUpgrades
                }
                break;
            case TabType.attributeUpgrades:
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
