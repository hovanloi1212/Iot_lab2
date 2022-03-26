using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderControl : MonoBehaviour
{
    public Slider slider;

    public void Minvalue()
    {
        slider.value = slider.minValue;
    }

    // Update is called once per frame
    public void Maxvalue() 
    {
        slider.value = slider.maxValue;
    }
}
