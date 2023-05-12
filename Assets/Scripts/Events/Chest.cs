using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Event
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        CheckBeginInput();
    }

    protected override void Begin()
    {
        EnemyManager.Instance.StartRushWithTimer(LengthInSeconds);
        base.Begin();
        EndCondition = true;
    }

    protected override void End()
    {
        LevelManager.Instance.GainXenorium((int)Random.Range(100, 201));
        LevelManager.Instance.GainNovacite((int)Random.Range(100, 201));
        LevelManager.Instance.GainVoidStone((int)Random.Range(100, 201));
        LevelManager.Instance.SpawnAugmentChoice();
        LevelManager.Instance.SpawnAttributeChoice();
        base.End();
    }
}
