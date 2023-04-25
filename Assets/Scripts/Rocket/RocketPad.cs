using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RocketPad : MonoBehaviour
{
    public bool readyToActivate;
    [SerializeField] private PlayerInput controls;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.started += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!readyToActivate) return;
        //TODO: Escape/Win/TriggerFinalSequence
        Debug.Log("Congrats!");
        SceneManager.LoadScene("VictoryScene");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        if (!RocketManager.canBuildEscapeRocket()) return;
        readyToActivate = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        if (!RocketManager.canBuildEscapeRocket()) return;
        readyToActivate = false;
    }
}