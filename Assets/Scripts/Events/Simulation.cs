using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Simulation : Event
{
    private Cannon playerCannonScript;
    private bool checkForWeapons;
    [field: SerializeField] private TextMeshProUGUI text { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        playerCannonScript = LevelManager.Instance.player.GetComponent<BaseFunctions>().cannon;
        checkForWeapons = true;
    }

    void Update()
    {
        CheckBeginInput();
        if (checkForWeapons)
        {
            if (playerCannonScript.lockedCannonProjectiles.Count == 0)
            {
                checkForWeapons = false;
                text.text = "All weapons acquired";
            }
        }
    }

    protected override void Begin()
    {
        if (checkForWeapons)
        {
            playerCannonScript.UnlockRandomCannon();
            WeaponWheelButtonController.UpdateUnlockedCannons();
            playerCannonScript.SetSwitchingEnabledState(false);
            EnemyManager.Instance.StartRushWithTimer(LengthInSeconds);
            LevelCanvasManager.Instance.StartTimer(LengthInSeconds);
            base.Begin();
        }
    }

    protected override void End()
    {
        playerCannonScript.SetSwitchingEnabledState(true);
        base.End();
    }
}
