using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RocketManager
{
    // TODO: THIS SHOULD BE COMBINED INTO THE LEVELMANAGER OR SIMILAR
    public static int rocketPartsCollected { get; set; }
    public static int requiredRocketPartsToEscape = 3;

    public static void collectRocketPart()
    {
        if (rocketPartsCollected >= requiredRocketPartsToEscape) return;
        rocketPartsCollected++;
        LevelCanvasManager.Instance.SetRocketPartAmount(rocketPartsCollected);
    }

    public static bool canBuildEscapeRocket() => rocketPartsCollected >= requiredRocketPartsToEscape;
}