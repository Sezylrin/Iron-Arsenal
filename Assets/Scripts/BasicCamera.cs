using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 CurrentPos;
    private Vector3 Rotation;
    private Camera cam;
    public Transform player;
    private Vector3 pos;
    void Start()
    {
        if (!player)
            player = GameObject.FindWithTag("Player").transform;
        pos = transform.position;
        Rotation = transform.eulerAngles;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.position);
        //Debug.Log(pos);
        if (LevelManager.Instance.currentState == State.Normal)
        {
            cam.orthographic = false;
            cam.transform.position = player.position + pos;
            transform.eulerAngles = Rotation;
        }
        else
        {
            cam.orthographic = true;
            cam.orthographicSize = 10;
            transform.position = (Vector3.up * 7) + player.position;
            transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
