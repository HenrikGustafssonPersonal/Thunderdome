using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    private AudioSource source;
    [SerializeField]
    private float maxVolume;
    [SerializeField]
    private float minVolume;

    public bool increaseVolume = false;

    public bool transitionInProgress;
    public float transitionDeathTimer = 1.0f;
    private float currentTransitionTimer;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (increaseVolume)
        {
            source.volume = AudioManager.Instance.targetMusicVolume*minVolume*5;
            currentTransitionTimer = AudioManager.Instance.targetMusicVolume * minVolume * 5;
        }
        else
        {
            source.volume = AudioManager.Instance.targetMusicVolume * maxVolume * 5;
            currentTransitionTimer = AudioManager.Instance.targetMusicVolume * maxVolume * 5;
        }
    }

    private void Update()
    {
        if (transitionInProgress)
        {
            if (increaseVolume)
            {
                currentTransitionTimer += AudioManager.Instance.targetMusicVolume * Time.deltaTime;
                source.volume = currentTransitionTimer;
                if (currentTransitionTimer- AudioManager.Instance.targetMusicVolume * minVolume * 5 >= transitionDeathTimer)
                    Destroy(gameObject);
            }
            else
            {
                currentTransitionTimer -= AudioManager.Instance.targetMusicVolume * Time.deltaTime;
                source.volume = currentTransitionTimer;
                if (currentTransitionTimer <= 0)
                    Destroy(gameObject);
            }
        }
    }
}
