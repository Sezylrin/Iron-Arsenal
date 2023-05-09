using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHunt : Event
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        CheckBeginInput();

        if (Active)
        {
            if (!EnemyManager.Instance.IsBossAlive)
            {
                Condition = true;
            }
        }
    }

    protected override void Begin()
    {
        EnemyManager.Instance.SpawnEnemy(true);
        base.Begin();
    }
}
