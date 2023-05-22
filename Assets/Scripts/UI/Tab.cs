using UnityEngine;
using UnityEngine.UI;

public enum TabType
{
    mechUpgrades,
    attributeUpgrades,
    augmentPurchases,
    sentryPurchases
}

public class Tab : MonoBehaviour
{
    [SerializeField] private Sprite activeImg;
    [SerializeField] private Sprite inactiveImg;

    public TabType tabType;

    public void ClickTab()
    {
        gameObject.GetComponent<Image>().sprite = activeImg;
    }

    public void DeactivateTab()
    {
        gameObject.GetComponent<Image>().sprite = inactiveImg;
    }
}
