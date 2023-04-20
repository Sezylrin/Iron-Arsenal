using UnityEngine;
using System.ComponentModel;
public enum AugmentName
{
    [Description("ExplosiveProjectiles")]
    ExplosiveProjectiles,
    [Description("PiercingBullets")]
    PiercingBullets,
    [Description("ExplosionSize")]
    ExplosionSize,
    [Description("Knockback")]
    Knockback,
    [Description("ExplosiveEnemies")]
    ExplosiveEnemies,
    [Description("FieryBullets")]
    FieryBullets,
    [Description("FireTrails")]
    FireTrails,
    [Description("FireSpread")]
    FireSpread,
    [Description("LowerResistance")]
    LowerResistance,
    [Description("PoisonBullets")]
    PoisonBullets,
    [Description("Poison")]
    Poison,
    [Description("DeathByPoison")]
    DeathByPoison,
    [Description("FrozenBullets")]
    FrozenBullets,
    [Description("Freeze")]
    Freeze,
    [Description("Frozensplosion")]
    Frozensplosion,
    [Description("RangeUp")]
    RangeUp,
    [Description("BossDamage")]
    BossDamage,
    [Description("Thorns")]
    Thorns,
    [Description("ShieldRegen")]
    ShieldRegen,
    [Description("ShieldUp")]
    ShieldUp,
    [Description("Shieldsteal")]
    Shieldsteal,
    [Description("Lifesteal")]
    Lifesteal,
}

[CreateAssetMenu(fileName = "AugmentData", menuName = "ScriptableObjects/AugmentData")]
public class AugmentData : ScriptableObject
{
    public Sprite icon;
    public string augName;
    public string description;

    public Augments augmentType;
}
