using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 CurrentPos;
    private Vector3 Rotation;
    private Camera cam;
    void Start()
    {
        CurrentPos = transform.localPosition;
        Rotation = transform.eulerAngles;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.currentState == State.Normal)
        {
            cam.orthographic = false;
            transform.localPosition = CurrentPos;
            transform.eulerAngles = Rotation;
        }
        else
        {
            cam.orthographic = true;
            cam.orthographicSize = 10;
            transform.localPosition = Vector3.up * 7;
            transform.eulerAngles = new Vector3(90, 0, 0);  
        }
    }
}
