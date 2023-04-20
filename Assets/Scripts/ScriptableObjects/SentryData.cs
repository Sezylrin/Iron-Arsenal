
using UnityEngine;
using System.ComponentModel;

public enum SentryName
{
    [Description("BasicSentry")]
    BasicSentry,
    FlameThrower,
    FreezeTower
}

[CreateAssetMenu(fileName = "SentryData",menuName ="ScriptableObjects/SentryData")]
public class SentryData : ScriptableObject
{
    public SentryName Sentry;
    public StatAttribute Attribute;
    public Material Colour;

    public float range;
    public float losAngle;
    public float fireRate;

    public ProjectileData projectileData;

    public int ironCost;
    public int copperCost;
    public int goldCost;
    public bool buildable;
}
