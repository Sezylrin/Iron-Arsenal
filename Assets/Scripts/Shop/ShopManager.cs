using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public MechUpgradeData[] mechUpgrades = new MechUpgradeData[3];
    public AttributeUpgradeData[] attributeUpgrades = new AttributeUpgradeData[3];
    private List<SentryData> purchasableSentries = new();

    private bool collidingWithPlayer = false;
    public TabType currTab = TabType.mechUpgrades;

    private void Start()
    {
        List<SentryData> lockedSentries = new List<SentryData>(SentryManager.Instance.LockedSentries);
        int numSentriesToChoose = 3;

        if (numSentriesToChoose > lockedSentries.Count)
        {
            numSentriesToChoose = lockedSentries.Count;
        }
        for (int i = 0; i < numSentriesToChoose; i++)
        {
            int randomIndex = Random.Range(0, lockedSentries.Count);
            purchasableSentries.Add(lockedSentries[randomIndex]);
            lockedSentries.RemoveAt(randomIndex);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && collidingWithPlayer)
        {
            OpenShop();
        }
    }

    private void OpenShop()
    {
        for (int i = purchasableSentries.Count - 1; i >= 0; i--)
        {
            if (!SentryManager.Instance.LockedSentries.Contains(purchasableSentries[i]))
            {
                purchasableSentries.RemoveAt(i);
            }
        }
        LevelCanvasManager.Instance.OpenShopMenu(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collidingWithPlayer)
        {
            collidingWithPlayer = true;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && collidingWithPlayer)
        {
            collidingWithPlayer = false;
        }
    }

    public void PurchaseSentry(SentryData sentryData)
    {
        purchasableSentries.Remove(sentryData);
    }

    public List<SentryData> GetPurchasableSentries()
    {
        return purchasableSentries;
    }
}
