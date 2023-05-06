using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{   
    public Slider slider;

    public void SetMaxSliderValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetSliderAmount(float sliderAmount)
    {
        slider.value = sliderAmount;
    }
}
