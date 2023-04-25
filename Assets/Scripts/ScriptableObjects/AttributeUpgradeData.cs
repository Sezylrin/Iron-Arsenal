using UnityEngine;

public enum Attribute
{
    Health,
    Physical,
    Elemental,
}

[CreateAssetMenu(fileName = "AttributeUpgradeData", menuName = "ScriptableObjects/AttributeUpgradeData")]
public class AttributeUpgradeData : ScriptableObject
{
    public Attribute attribute;

    public Sprite icon;
    public string upgradeName;
    public string description;

    public int xenoriumCost;
    public int novaciteCost;
    public int voidStoneCost;
}
