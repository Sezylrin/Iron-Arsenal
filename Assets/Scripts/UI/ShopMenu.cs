using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [Header("Tab Buttons")]
    [SerializeField] private GameObject mechUpgradesTab;
    [SerializeField] private GameObject attributeUpgradesTab;
    [SerializeField] private GameObject augmentUpgradesTab;
    [SerializeField] private GameObject sentryPurchaseTab;

    [Header("Resources")]
    [SerializeField] private TMP_Text xenorium;
    [SerializeField] private TMP_Text novacite;
    [SerializeField] private TMP_Text voidStone;

    [Header("PurchaseItems")]
    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;
    [SerializeField] private GameObject item3;

    [Header("Misc")]
    [SerializeField] private Button closeBtn;

    private ShopManager shopManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    private void Start()
    {
        Tab upgradeTab = mechUpgradesTab.GetComponent<Tab>();
        mechUpgradesTab.GetComponent<Button>().onClick.AddListener(() => HandleClickTab(upgradeTab));
        upgradeTab.tabType = TabType.mechUpgrades;

        Tab attrTab = attributeUpgradesTab.GetComponent<Tab>();
        attributeUpgradesTab.GetComponent<Button>().onClick.AddListener(() => HandleClickTab(attrTab));
        attrTab.tabType = TabType.attributeUpgrades;

        Tab augTab = augmentUpgradesTab.GetComponent<Tab>();
        augmentUpgradesTab.GetComponent<Button>().onClick.AddListener(() => HandleClickTab(augTab));
        augTab.tabType = TabType.augmentPurchases;

        Tab sentryTab = sentryPurchaseTab.GetComponent<Tab>();
        sentryPurchaseTab.GetComponent<Button>().onClick.AddListener(() => HandleClickTab(sentryTab));
        sentryTab.tabType = TabType.sentryPurchases;

        closeBtn.onClick.AddListener(CloseMenu);
    }

    public void OpenMenu(ShopManager shopManager)
    {
        ResourcesAmount resources = LevelManager.Instance.GetResources();
        xenorium.text = resources.xenorium.ToString();
        novacite.text = resources.novacite.ToString();
        voidStone.text = resources.voidStone.ToString();
        this.shopManager = shopManager;

        if (shopManager.GetPurchasableSentries().Count == 0)
        {
            sentryPurchaseTab.SetActive(false);
        }
        else
        {
            sentryPurchaseTab.SetActive(true);
        }
        DisplayItems(FindCurrentTab());
    }

    private void CloseMenu()
    {
        Tab mechUpgradesTabComponent = mechUpgradesTab.GetComponent<Tab>();
        Tab sentryPurchaseTabComponent = sentryPurchaseTab.GetComponent<Tab>();
        shopManager.currTab = TabType.mechUpgrades;
        HandleClickTab(mechUpgradesTab.GetComponent<Tab>());
        LevelCanvasManager.Instance.CloseShopMenu();
        sentryPurchaseTabComponent.DeactivateTab();
        mechUpgradesTabComponent.ClickTab();
    }

    private void HandleClickTab(Tab clickedTab)
    {
        if (shopManager.currTab != clickedTab.tabType)
        {
            clickedTab.ClickTab();
            DisplayItems(clickedTab);
            FindCurrentTab().DeactivateTab();
            shopManager.currTab = clickedTab.tabType;
        }
    }

    private void DisplayItems(Tab currTab)
    {
        switch (currTab.tabType) {
            case TabType.mechUpgrades:
                item1.GetComponent<PurchaseItem>().SetMechUpgrade(shopManager.mechUpgrades[0], TabType.mechUpgrades, this);
                item1.SetActive(true);
                item2.GetComponent<PurchaseItem>().SetMechUpgrade(shopManager.mechUpgrades[1], TabType.mechUpgrades, this);
                item2.SetActive(true);
                item3.GetComponent<PurchaseItem>().SetMechUpgrade(shopManager.mechUpgrades[2], TabType.mechUpgrades, this);
                item3.SetActive(true);
                break;
            case TabType.attributeUpgrades:
                DisplayAttributePurchases();
                break;
            case TabType.augmentPurchases:
                DisplayAugmentPurchases();
                break;
            case TabType.sentryPurchases:
                DisplaySentryPurchases();
                break;
        }
    }

    public void PurchaseAttribute(AttributeUpgradeData attrUpgrade)
    {
        shopManager.PurchaseAttributeUpgrade(attrUpgrade);
        UpdateResources();

        if (shopManager.GetAttributeUpgradesRemaining().Count == 0)
        {
            HandleClickTab(mechUpgradesTab.GetComponent<Tab>());
            attributeUpgradesTab.SetActive(false);
        }

        DisplayItems(FindCurrentTab());
    }

    public void PurchaseAugment(AugmentData augment)
    {
        shopManager.PurchaseAugment(augment);
        UpdateResources();

        if (shopManager.GetPurchasableAugments().Count == 0)
        {
            HandleClickTab(mechUpgradesTab.GetComponent<Tab>());
            augmentUpgradesTab.SetActive(false);
        }

        DisplayItems(FindCurrentTab());
    }

    public void PurchaseSentry(SentryData sentry)
    {
        shopManager.PurchaseSentry(sentry);
        UpdateResources();

        if (shopManager.GetPurchasableSentries().Count == 0)
        {
            HandleClickTab(mechUpgradesTab.GetComponent<Tab>());
            sentryPurchaseTab.SetActive(false);
        }

        DisplayItems(FindCurrentTab());
    }

    public void PurchaseItem()
    {
        UpdateResources();
        DisplayItems(FindCurrentTab());
    }

    public void UpdateResources()
    {
        xenorium.text = LevelManager.Instance.GetXenorium().ToString();
        novacite.text = LevelManager.Instance.GetNovacite().ToString();
        voidStone.text = LevelManager.Instance.GetVoidStone().ToString();
    }

    private void DisplayAttributePurchases()
    {
        List<AttributeUpgradeData> purchasableAttributes = shopManager.GetAttributeUpgradesRemaining();
        item1.GetComponent<PurchaseItem>().SetAttributeUpgrade(purchasableAttributes[0], TabType.attributeUpgrades, this);

        if (purchasableAttributes.Count == 1)
        {
            item2.SetActive(false);
            item3.SetActive(false);
        }
        else if (purchasableAttributes.Count == 2)
        {
            item2.GetComponent<PurchaseItem>().SetAttributeUpgrade(purchasableAttributes[1], TabType.attributeUpgrades, this);
            item3.SetActive(false);
        }
        else if (purchasableAttributes.Count == 3)
        {
            item2.GetComponent<PurchaseItem>().SetAttributeUpgrade(purchasableAttributes[1], TabType.attributeUpgrades, this);
            item3.GetComponent<PurchaseItem>().SetAttributeUpgrade(purchasableAttributes[2], TabType.attributeUpgrades, this);
        }
    }

    private void DisplayAugmentPurchases()
    {
        List<AugmentData> purchasableAugments = shopManager.GetPurchasableAugments();
        item1.GetComponent<PurchaseItem>().SetAugmentPurchase(purchasableAugments[0], TabType.augmentPurchases, this);

        if (purchasableAugments.Count == 1)
        {
            item2.SetActive(false);
            item3.SetActive(false);
        }
        else if (purchasableAugments.Count == 2)
        {
            item2.GetComponent<PurchaseItem>().SetAugmentPurchase(purchasableAugments[1], TabType.sentryPurchases, this);
            item3.SetActive(false);
        }
        else if (purchasableAugments.Count == 3)
        {
            item2.GetComponent<PurchaseItem>().SetAugmentPurchase(purchasableAugments[1], TabType.sentryPurchases, this);
            item3.GetComponent<PurchaseItem>().SetAugmentPurchase(purchasableAugments[2], TabType.sentryPurchases, this);
        }
    }

    private void DisplaySentryPurchases()
    {
        List<SentryData> purchasableSentries = shopManager.GetPurchasableSentries();
        item1.GetComponent<PurchaseItem>().SetSentryPurchase(purchasableSentries[0], TabType.sentryPurchases, this);
        
        if (purchasableSentries.Count == 1)
        {
            item2.SetActive(false);
            item3.SetActive(false);
        }
        else if (purchasableSentries.Count == 2)
        {
            item2.GetComponent<PurchaseItem>().SetSentryPurchase(purchasableSentries[1], TabType.sentryPurchases, this);
            item3.SetActive(false);
        }
        else if (purchasableSentries.Count == 3)
        {
            item2.GetComponent<PurchaseItem>().SetSentryPurchase(purchasableSentries[1], TabType.sentryPurchases, this);
            item3.GetComponent<PurchaseItem>().SetSentryPurchase(purchasableSentries[2], TabType.sentryPurchases, this);
        }
    }

    private Tab FindCurrentTab()
    {
        Tab mechUpgTab = mechUpgradesTab.GetComponent<Tab>();
        if (shopManager.currTab == mechUpgTab.tabType) {
            return mechUpgTab;
        }

        Tab attrUpgTab = attributeUpgradesTab.GetComponent<Tab>();
        if (shopManager.currTab == attrUpgTab.tabType)
        {
            return attrUpgTab;
        }

        Tab augUpgTab = augmentUpgradesTab.GetComponent<Tab>();
        if (shopManager.currTab == augUpgTab.tabType)
        {
            return augUpgTab;
        }

        Tab sentryPurchTab = sentryPurchaseTab.GetComponent<Tab>();
        if (shopManager.currTab == sentryPurchTab.tabType)
        {
            return sentryPurchTab;
        }
        return mechUpgTab;
    }
}
