
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
    public Material Colour;

    public float range;
    public float losAngle;
    public float fireRate;

    public ProjectileData projectileData;

    public int ironCost;
    public int copperCost;
    public bool buildable;
}
