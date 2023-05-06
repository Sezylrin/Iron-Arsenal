using UnityEngine;

public enum UpgradeType
{
    health,
    drill,
    baseSize,
}

[CreateAssetMenu(fileName = "MechUpgradeData", menuName = "ScriptableObjects/MechUpgradeData")]
public class MechUpgradeData : ScriptableObject
{
    public UpgradeType upgradeType;
    public Sprite icon;
    public string upgradeName;
    public string description;

    public int xenoriumCost;
    public int novaciteCost;
    public int voidStoneCost;
}
