using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
        [Header("Walk")]
    public float maxSpeed;
    public float moveAccel;
    public float moveDecel;
    [Range(0.1f, 2f)] public float accelPower;
    public bool allowStopPower;
    [Range(0.1f, 2f)] public float stopPower;
    public float rotateSpeed;

    [Header("Friction")] public float frictionAmount;
}