using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Event
{
    [field: SerializeField] private int RushLength { get; set; }
    
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
        EnemyManager.Instance.StartRushWithTimer(RushLength);
        base.Begin();
        EndCondition = true;
    }

    protected override void End()
    {
        LevelManager.Instance.GainXenorium((int)Random.Range(50, 101));
        LevelManager.Instance.GainNovacite((int)Random.Range(50, 151));
        LevelManager.Instance.GainVoidStone((int)Random.Range(50, 151));
        if (Random.Range(0,2) == 0)
        LevelManager.Instance.SpawnAugmentChoice();
        else
        LevelManager.Instance.SpawnAttributeChoice();
        base.End();
    }
}
