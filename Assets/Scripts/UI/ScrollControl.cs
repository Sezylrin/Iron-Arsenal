using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
    public Scrollbar scrollbarHorizontal;
    public Button scrollLeftButton;
    public Button scrollRightButton;
    public float scrollAmount = 0.1f;

    private void Start()
    {
        scrollLeftButton.onClick.AddListener(ScrollLeft);
        scrollRightButton.onClick.AddListener(ScrollRight);
    }

    private void ScrollLeft()
    {
        float newValue = Mathf.Clamp(scrollbarHorizontal.value - scrollAmount, 0, 1);
        scrollbarHorizontal.value = newValue;
    }

    private void ScrollRight()
    {
        float newValue = Mathf.Clamp(scrollbarHorizontal.value + scrollAmount, 0, 1);
        scrollbarHorizontal.value = newValue;
    }
}
