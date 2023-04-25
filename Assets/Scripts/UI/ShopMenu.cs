using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("Tab Buttons")]
    [SerializeField] private Button upgradesTab;
    [SerializeField] private Button statsTab;
    [SerializeField] private Button augmentsTab;

    [SerializeField] private Button closeBtn;

    private Tab selectedTab;

    private void Start()
    {
        upgradesTab.onClick.AddListener(() => HandleClickTab(upgradesTab.GetComponent<Tab>()));
        statsTab.onClick.AddListener(() => HandleClickTab(statsTab.GetComponent<Tab>()));
        augmentsTab.onClick.AddListener(() => HandleClickTab(augmentsTab.GetComponent<Tab>()));
        closeBtn.onClick.AddListener(() => LevelManager.Instance.CloseShopMenu());

        selectedTab = upgradesTab.GetComponent<Tab>();
    }

    private void HandleClickTab(Tab clickedTab)
    {
        if (selectedTab != clickedTab)
        {
            clickedTab.ClickTab();
            selectedTab.DeactivateTab();
            selectedTab = clickedTab;
        }
    }
}
