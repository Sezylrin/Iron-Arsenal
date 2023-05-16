using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RocketPart : Event
{
    [field: SerializeField] private int RushLength { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBeginInput();

        if (Active)
        {
            if (!EnemyManager.Instance.IsBossAlive)
            {
                EndCondition = true;
            }
        }
    }

    protected override void Begin()
    {
        EnemyManager.Instance.StartRushWithTimer(RushLength);
        EnemyManager.Instance.SpawnEnemy(true);
        base.Begin();
    }

    protected override void End()
    {
        RocketManager.collectRocketPart();
        base.End();
    }
}