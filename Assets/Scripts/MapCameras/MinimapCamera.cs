using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField, Header("For testing, should auto grab player if LevelManager in scene")] private Transform player;
    public float playerYOffset = 12f;
    public bool allowMinimapRotation;

    private void Start()
    {
        if (player) return;
        player = LevelManager.Instance.player.transform;
    }

    void Update()
    {
        if (!player) return;
        transform.position = new Vector3(player.transform.position.x, player.position.y + playerYOffset, player.position.z);
        if (!allowMinimapRotation) return;
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}