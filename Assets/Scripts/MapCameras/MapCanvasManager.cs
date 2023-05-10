using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCanvasManager : MonoBehaviour
{
    [Header("Copy this to another manager and set the mapUI and event stuff")]
    [Header("Read the README if I'm not around")]
    public GameObject mapUI;

    public void ToggleMap(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!mapUI.activeSelf)
            {
                mapUI.SetActive(true);
            }
            else
            {
                CloseMap();
            }
        }
    }

    public void CloseMap()
    {
        mapUI.SetActive(false);
    }
}