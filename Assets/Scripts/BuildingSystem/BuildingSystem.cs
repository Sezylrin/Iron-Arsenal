using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Utils;

public class BuildingSystem : MonoBehaviour
{
    public Transform TestPrefab;
    [SerializeField]
    private LayerMask MouseLayerMask = -1;
    private Vector3 MousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = MousePosition.MouseToWorld3D(Camera.main, MouseLayerMask);
        Debug.Log(MousePos);
        transform.position = MousePos;
    }
}
