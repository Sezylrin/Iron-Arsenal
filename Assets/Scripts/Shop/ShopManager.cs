using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private bool initialised;
    private bool collidingWithPlayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && collidingWithPlayer)
        {
            LevelManager.Instance.OpenShopMenu();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collidingWithPlayer = true;   
        }
    }
}
