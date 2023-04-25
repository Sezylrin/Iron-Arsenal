using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public MechUpgradeData[] mechUpgrades = new MechUpgradeData[3];
    public AttributeUpgradeData[] attributeUpgrades = new AttributeUpgradeData[3];
    public List<SentryData> purchasableSentries { get; private set; } = new();

    private bool collidingWithPlayer = false;
    public TabType currTab = TabType.mechUpgrades;

    private void Start()
    {
        List<SentryData> lockedSentries = SentryManager.Instance.LockedSentries;
        int numSentriesToChoose = 3;

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
            LevelCanvasManager.Instance.OpenShopMenu(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collidingWithPlayer = true;   
        }
    }

    public void PurchaseSentry(SentryData sentryData)
    {
        purchasableSentries.Remove(sentryData);
    }
}
