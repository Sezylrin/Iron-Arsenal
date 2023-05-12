using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MousePosition
{
    //public static MousePosition Instance { get; private set; }

    public static Vector3 MouseToWorld3D(Camera WorldCamera, LayerMask Mask)
    {
        Ray ray = WorldCamera.ScreenPointToRay(Input.mousePosition);        
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Mask))
            return hit.point;
        else
            return Vector3.zero;
    }

    public static Vector3 MouseToWorld2D(Camera WorldCamera)
    {
        return WorldCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}

