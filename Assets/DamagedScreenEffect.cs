using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class DamagedScreenEffect : MonoBehaviour
{
    //Converts to max opacity that damaged screen effect will pulse to. 
    public float pulsePeakOpacity;
    public float pulseMinOpacity;
    public float screenDamagedEffectPulseInterval;
    public float opacityIncreaseRate = 0.01f;

    public float currentOpacity = 0;
    public bool declining = false;

    public Image damagedScreenImage;

    void Start()
    {
     
        pulsePeakOpacity = 0.0f;
        pulseMinOpacity = 0.0f;
        screenDamagedEffectPulseInterval = 1f;
    }
    void Update()
    {
        PulseScreenEffect();

    }


    //Peak pulse is set when player is damaged, screen effect will pulse to value.
    //Value should increase as player is damaged.
    public void SetPeakMinPulseOpacity(float minValue, float maxValue, float intensity)
    {
        pulsePeakOpacity = maxValue;
        pulseMinOpacity = minValue;
        //FIX
        opacityIncreaseRate = intensity;


    }
    void PulseScreenEffect()
    {

        var tempColor = damagedScreenImage.color;
        var startValue = damagedScreenImage.color;

        startValue.a = pulseMinOpacity;
        tempColor.a = currentOpacity;

        damagedScreenImage.color = tempColor;
        //Loop infinitely
        if (pulsePeakOpacity == 0 && pulseMinOpacity == 0 && currentOpacity > 0)
        {
            currentOpacity =0;
        }
        if (pulsePeakOpacity == 0)
        {
            return;
        }
        if (!declining && currentOpacity <= pulsePeakOpacity)
        {
            currentOpacity += opacityIncreaseRate*Time.deltaTime;
        } 
        else if (!declining && currentOpacity >= pulsePeakOpacity) 
        { 
            declining = true;
        }
        else if (declining && currentOpacity <= pulseMinOpacity)
            declining = false;
        else if(declining) 
        { 
            currentOpacity -= opacityIncreaseRate*Time.deltaTime;
        }
     
      


    }

    public float GetCurrentScreenDamageOpacity()
    {
        return currentOpacity;
    }
}
