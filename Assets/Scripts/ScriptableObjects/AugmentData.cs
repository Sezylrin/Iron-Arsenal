using UnityEngine;
using System.ComponentModel;
public enum AugmentName
{
    [Description("ExplosiveAugment")]
    ExplosiveAugment,
    [Description("FrostAugment")]
    FrostAugment,
    [Description("PoisonAugment")]
    PoisonAugment,
}

[CreateAssetMenu(fileName = "AugmentData", menuName = "ScriptableObjects/AugmentData")]
public class AugmentData : ScriptableObject
{
    public Sprite icon;
    public string augName;
    public string description;

    public Augments augmentType;

    public int mode;
}
