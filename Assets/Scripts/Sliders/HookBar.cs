using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookBar : MonoBehaviour
{
    public Slider slider;
    private Vector3 originalPos;
    private void Start()
    {
        originalPos = transform.position;

    }
    public void SetMaxHookCooldown(float maxValue)
    {

        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetCooldown(float coolDown)
    {
        slider.value = coolDown;

    }

    public void adjustMovement(Vector3  movementÍnX, Vector3 movementInY)
    {
        //transform.position = originalPos - movementÍnX * 4;
        //transform.position = originalPos + movementInY * 4;
    }
}
