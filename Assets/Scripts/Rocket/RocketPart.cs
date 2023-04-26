using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RocketPart : MonoBehaviour
{
    public new string name;
    [Serializable]
    public struct RequiredMaterials
    {
        public int novaciteAmount;
        public int voidstoneAmount;
        public int xenoriumAmount;
    }
    public RequiredMaterials requiredMaterials;
    public float timeToCraft;
    // public Image image;
    [SerializeField] public TextMeshProUGUI text;

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
        controls = LevelManager.Instance.player.GetComponent<PlayerInput>();
        var interactAction = controls.currentActionMap.FindAction("Interact");
        interactAction.performed += OnInteractDown;
        interactAction.canceled += OnInteractUp;
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
        interactAction.performed -= OnInteractDown;
        interactAction.canceled -= OnInteractUp;
    }

    private void OnInteractDown(InputAction.CallbackContext context)
    {
        if (!canvas.enabled) return;
        //TODO: Add an error response
        if(LevelManager.Instance.PurchaseItemIfPossible(requiredMaterials.xenoriumAmount, requiredMaterials.novaciteAmount, requiredMaterials.voidstoneAmount))
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

    private void OnTriggerStay(Collider other)
    {
        OrientCanvasToCamera();
        UpdateTextGUI();
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
        text.text = name + "\n " +
                    "Novacite: " + LevelManager.Instance.buildManager.novacite + "/" + requiredMaterials.novaciteAmount + "\n" +
                    "Voidstone: " + LevelManager.Instance.buildManager.voidStone + "/" + requiredMaterials.voidstoneAmount + "\n" +
                    "Xenorium: " + LevelManager.Instance.buildManager.xenorium + "/" + requiredMaterials.xenoriumAmount + "\n" +
                    "Hold 'E' to craft";
    }
}