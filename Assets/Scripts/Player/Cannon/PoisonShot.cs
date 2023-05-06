using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShot : CannonProjectile
{

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Init()
    {
        Vector3 tempPos = transform.position;
        tempPos.y = 0;
        Vector3 tempMouse = mousePos;
        tempMouse.y = 0;
        Shoot(tempMouse - tempPos);
        base.Init();
    }
}
