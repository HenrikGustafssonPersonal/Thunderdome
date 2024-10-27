using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float volume;
    public float FOV;
    public float sensitivity;
    public float gameTimer;


    public bool timeON = false;

    private void OnLevelWasLoaded(int level)
    {
        // As soon as level has swapped time will start:
        timeON = true;

        // Try to set values if possible:
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.targetMusicVolume = volume;

            // If tutorial level, play pause music:
            if (level == 2) 
                AudioManager.Instance.tier1MusicOn = true;
            else
                AudioManager.Instance.tier1MusicOn = false;
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Camera[] cameras = player.GetComponentsInChildren<Camera>();
            Debug.Log(cameras.Length + " Cameras found");
            foreach (Camera c in cameras)
            {
                c.fieldOfView = FOV;
            }
            player.GetComponentInChildren<MouseLook>().mouseSensitivity = sensitivity;
        }



    }

    private void Update()
    {
        if(timeON)
            gameTimer += Time.deltaTime;

    }

}
