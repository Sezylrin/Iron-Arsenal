using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RocketPad : MonoBehaviour
{
    private bool readyToActivate;
    [SerializeField] private PlayerInput controls;
    [SerializeField] private TextMeshProUGUI text;
    private Canvas canvas;
    private Transform cameraTransform;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        cameraTransform = FindObjectOfType<Camera>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        controls = LevelManager.Instance.player.GetComponent<PlayerInput>();
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.started += OnInteract;
        UpdateTextGUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        if (!controls) return;
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!readyToActivate) return;
        EnemyManager.Instance.StartBossRush();
        //Debug.Log("Congrats!");
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        canvas.enabled = true;
        if (!RocketManager.canBuildEscapeRocket()) return;
        readyToActivate = true;
    }

    private void OnTriggerStay(Collider other)
    {
        OrientCanvasToCamera();
        UpdateTextGUI();
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        canvas.enabled = false;
        if (!RocketManager.canBuildEscapeRocket()) return;
        readyToActivate = false;
    }

    private void OrientCanvasToCamera()
    {
        if (!canvas.enabled) return;
        canvas.transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(transform.position.x,
                                                                                            cameraTransform.position.y,
                                                                                            cameraTransform.position.z));
    }

    private void UpdateTextGUI()
    {
        // TODO: Replace this with their sprite instead
        text.text = "Rocket Components Collected: " + RocketManager.rocketPartsCollected + "/" + RocketManager.requiredRocketPartsToEscape + "\n" +
                    "Press 'E' to launch";
    }
}