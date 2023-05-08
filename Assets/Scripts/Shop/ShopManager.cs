using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public MechUpgradeData[] mechUpgrades = new MechUpgradeData[3];
    public AttributeUpgradeData[] attributeUpgrades = new AttributeUpgradeData[3];
    private List<SentryData> purchasableSentries = new();
    private List<AugmentData> purchasableAugments = new();
    public GameObject canvas;

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

        List<AugmentData> lockedAugments = new List<AugmentData>(AugmentManager.Instance.allAugments);
        int numAugmentsToChoose = 3;

        if (numAugmentsToChoose > lockedAugments.Count)
        {
            numAugmentsToChoose = lockedAugments.Count;
        }
        for (int i = 0; i < numAugmentsToChoose; i++)
        {
            int randomIndex = Random.Range(0, lockedAugments.Count);
            purchasableAugments.Add(lockedAugments[randomIndex]);
            lockedAugments.RemoveAt(randomIndex);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        
        for (int i = purchasableAugments.Count - 1; i >= 0; i--)
        {
            if (!AugmentManager.Instance.allAugments.Contains(purchasableAugments[i])) {
                purchasableAugments.RemoveAt(i);
            }
        }
        LevelCanvasManager.Instance.OpenShopMenu(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collidingWithPlayer)
        {
            collidingWithPlayer = true;
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && collidingWithPlayer)
        {
            collidingWithPlayer = false;
            canvas.SetActive(false);
        }
    }

    public void PurchaseSentry(SentryData sentryData)
    {
        purchasableSentries.Remove(sentryData);
    }

    public void PurchaseAugment(AugmentData augmentData)
    {
        purchasableAugments.Remove(augmentData);
    }

    public List<SentryData> GetPurchasableSentries()
    {
        return purchasableSentries;
    }

    public List<AugmentData> GetPurchasableAugments()
    {
        return purchasableAugments;
    }
}
