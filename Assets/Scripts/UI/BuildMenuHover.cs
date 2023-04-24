using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class BuildMenuHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        LevelCanvasManager.Instance.overMenu = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LevelCanvasManager.Instance.overMenu = false;
    }
}
