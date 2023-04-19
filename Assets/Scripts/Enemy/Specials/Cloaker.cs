using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloaker : Enemy
{
    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        Move();
    }
}
