using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Transform rotatePoint;
    private Vector3 mouseLocation;
    private Vector3 worldPosition;

    void Awake()
    {
        rotatePoint = GameObject.Find("Gun Rotation Point").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        mouseLocation = Input.mousePosition;
        mouseLocation.z += 90;

        worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);
        worldPosition.y = 0;

        rotatePoint.LookAt(worldPosition);
    }
}
