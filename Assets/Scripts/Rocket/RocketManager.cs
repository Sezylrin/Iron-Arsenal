using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RocketManager
{
    // TODO: THIS SHOULD BE COMBINED INTO THE LEVELMANAGER OR SIMILAR
    public static int rocketPartsCollected { get; private set; }
    public static int requiredRocketPartsToEscape = 3;

    public static void collectRocketPart() => rocketPartsCollected++;

    public static bool canBuildEscapeRocket() => rocketPartsCollected >= requiredRocketPartsToEscape;
}