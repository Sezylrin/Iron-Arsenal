using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{
    public GameObject basicEnemy;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

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

    protected override void OnDeath()
    {
        Manager.SpawnEnemy(false, 0, spawnPoint1.position);
        Manager.SpawnEnemy(false, 0, spawnPoint2.position);
        base.OnDeath();
    }
}
