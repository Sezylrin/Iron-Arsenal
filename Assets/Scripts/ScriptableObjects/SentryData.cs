
using UnityEngine;
using System.ComponentModel;

public enum SentryName
{
    [Description("BasicSentry")]
    BasicSentry,
    Gattling,
    Cannon,
    FlameThrower,
    FreezeTower,
    PoisonTower,
    IceShardTower,
    IncendiaryTower,
    Plasma
}

[CreateAssetMenu(fileName = "SentryData",menuName ="ScriptableObjects/SentryData")]
public class SentryData : ScriptableObject
{
    public SentryName Sentry;
    public StatAttribute Attribute;
    public Material Colour;
    public Sprite SentryIcon;
    public string sentryName;
    public string description;

    public float range;
    public float losAngle;
    public float fireRate;

    public ProjectileData projectileData;

    public int xenoriumCost;
    public int novaciteCost;
    public int voidStoneCost;
    public bool buildable;

    public Augments[] defaultAugment;

    public int unlockXenoriumCost;
    public int unlockNovaciteCost;
    public int unlockVoidStoneCost;
}
