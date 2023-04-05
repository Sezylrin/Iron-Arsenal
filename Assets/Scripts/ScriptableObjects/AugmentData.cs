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
    public string augName;
    public string description;
}
