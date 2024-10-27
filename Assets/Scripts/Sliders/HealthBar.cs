using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider targetHealthSlider;
    public Slider currentHealthSlider;


    public void SetMaxHealth(float maxHealth)
    {

        targetHealthSlider.maxValue = maxHealth;
        targetHealthSlider.value = maxHealth;
        currentHealthSlider.value = maxHealth;
        currentHealthSlider.maxValue = maxHealth;
    }

    public void SetTargetHealth(float health)
    {
        targetHealthSlider.value = health;

    }
    public void SetCurrentHealth(float health)
    {
        currentHealthSlider.value = health;

    }
}
