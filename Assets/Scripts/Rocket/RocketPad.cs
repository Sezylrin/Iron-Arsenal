using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RocketPad : Event
{
    [field: SerializeField] private int RushLength { get; set; }
    [field: SerializeField] private TextMeshProUGUI text { get; set; }

    public GameObject rocket;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        UpdateTextGUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBeginInput();

        if (Active)
        {
            if (!EnemyManager.Instance.IsBossAlive)
            {
                EndCondition = true;
            }
        }
    }

    protected override void CheckBeginInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanStart && !EventManager.Instance.EventActive && RocketManager.canBuildEscapeRocket())
        {
            rocket.SetActive(true);
            Begin();
            canvas.enabled = false;
        }
    }

    protected override void Begin()
    {
        EnemyManager.Instance.StartRushWithTimer(RushLength);
        EnemyManager.Instance.StartBossRush();
        base.Begin();
    }

    protected override void End()
    {
        base.End();
    }

    private void UpdateTextGUI()
    {
        // TODO: Replace this with their sprite instead
        text.text = RocketManager.rocketPartsCollected + "/" + RocketManager.requiredRocketPartsToEscape ;
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OrientCanvasToCamera();
            UpdateTextGUI();
        }
    }
}

