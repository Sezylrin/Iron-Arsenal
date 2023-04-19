using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketPart : MonoBehaviour
{
    public new string name;
    [Serializable]
    public struct RequiredMaterials
    {
        public int Ore1Amount;
        public int Ore2Amount;
        public int Ore3Amount;
    }
    [SerializeField] private RequiredMaterials requiredMaterials;
    [SerializeField] private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (levelManager.buildManager.iron < requiredMaterials.Ore1Amount) return;
        if (levelManager.buildManager.copper < requiredMaterials.Ore2Amount) return;
        if (levelManager.buildManager.gold < requiredMaterials.Ore3Amount) return;


    }
}