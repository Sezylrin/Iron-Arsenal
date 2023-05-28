using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutTrigger2 : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        TutorialManager.Instance.Sequence4();
        Destroy(gameObject);
    }
}