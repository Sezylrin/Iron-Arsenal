using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{   
    public Slider slider;

    public void SetMaxSliderValue(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetSliderAmount(int sliderAmount)
    {
        slider.value = sliderAmount;
    }
}
