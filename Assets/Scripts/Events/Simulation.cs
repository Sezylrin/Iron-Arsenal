using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : Event
{
    private Cannon playerCannonScript;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        playerCannonScript = LevelManager.Instance.player.GetComponent<BaseFunctions>().cannon;
    }

    void Update()
    {
        CheckBeginInput();
    }

    protected override void Begin()
    {
        playerCannonScript.UnlockRandomCannon();
        WeaponWheelButtonController.UpdateUnlockedCannons();
        playerCannonScript.SetSwitchingEnabledState(false);
        EnemyManager.Instance.StartRushWithTimer(LengthInSeconds);
        LevelCanvasManager.Instance.StartTimer(LengthInSeconds);
        base.Begin();
    }

    protected override void End()
    {
        playerCannonScript.SetSwitchingEnabledState(true);
        base.End();
    }
}
