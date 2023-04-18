
using UnityEngine;
using System.ComponentModel;

public enum SentryName
{
    [Description("BasicSentry")]
    BasicSentry,
}

[CreateAssetMenu(fileName = "SentryData",menuName ="ScriptableObjects/SentryData")]
public class SentryData : ScriptableObject
{
    public SentryName Sentry;
    public Material Colour;

    public float range;
    public float losAngle;
    public float fireRate;

    public ProjectileData projectileData;

    public int xenoriumCost;
    public int novaciteCost;
    public int voidStoneCost;
    public bool buildable;
}
