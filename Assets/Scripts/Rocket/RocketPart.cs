using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RocketPart : MonoBehaviour
{
    public new string name;
    [Serializable]
    public struct RequiredMaterials
    {
        public int xenoriumAmount;
        public int novaciteAmount;
        public int voidStoneAmount;
    }
    public RequiredMaterials requiredMaterials;
    public float timeToCraft;
    // public Image image;

    [SerializeField] private PlayerInput controls;
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
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.performed += OnInteractDown;
        interactAction.canceled += OnInteractUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvas.enabled) return;
        canvas.transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(transform.position.x,
                                                                                            cameraTransform.position.y,
                                                                                            cameraTransform.position.z));
    }

    private void OnDestroy()
    {
        if (!controls) return;
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.performed -= OnInteractDown;
        interactAction.canceled -= OnInteractUp;
    }

    private void OnInteractDown(InputAction.CallbackContext context)
    {
        if (!canvas.enabled) return;
        if (LevelManager.Instance.buildManager.xenorium < requiredMaterials.xenoriumAmount ||
            LevelManager.Instance.buildManager.novacite < requiredMaterials.novaciteAmount ||
            LevelManager.Instance.buildManager.voidStone < requiredMaterials.voidStoneAmount) return; //TODO: Add an error response

        StartCoroutine(CraftPart());
    }

    private void OnInteractUp(InputAction.CallbackContext context)
    {
        StopCoroutine(CraftPart());
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        canvas.enabled = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        canvas.enabled = false;
        StopCoroutine(CraftPart());
    }

    IEnumerator CraftPart()
    {
        // float timer = 0f; TODO: Add image filling for prompt as you hold

        yield return new WaitForSeconds(timeToCraft);

        RocketManager.collectRocketPart();
        Debug.Log("Rocket Part Collected"); // TODO: Add to UI
        Destroy(gameObject);

        yield return null;
    }
}