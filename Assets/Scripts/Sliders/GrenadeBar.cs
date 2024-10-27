using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeBar : MonoBehaviour
{
    public Slider slider;


    public void SetMaxGrenadeCooldown(float maxValue)
    {

        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetCooldown(float coolDown)
    {
        slider.value = coolDown;

    }

}
