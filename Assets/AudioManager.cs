using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class AudioManager : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float targetMusicVolume = 0.095f;

    [SerializeField]
    private float sfxVolume = 1.0f;

    [Range(0, 4)]
    public int currentMusicTrack = 1;
    private int prevTrack;

    [SerializeField]
    private float pitchDiff = 0.2f;

    public float musicSwapDuration = 1.0f;
    private float currentMusicSwapVal = 0.0f;

    public bool tier5MusicOn = false;
    public bool tier1MusicOn = false;

    public static AudioManager Instance;
    public Sound[] sfxSounds;
    public AudioSource[] musicSource;

    public AudioSource sfxSource;

    private CheerScore cheerScore;
    private float cheer_o_meter;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        cheerScore = GameManager.instance.gameObject.GetComponent<CheerScore>();
        cheer_o_meter = cheerScore.scoreMultiplier;

        PlayMusic(currentMusicTrack);
        sfxSource.volume = sfxVolume;
    }

    private void Update()
    {
        cheer_o_meter = cheerScore.scoreMultiplier;


        // Check if there is a swap in progress:
        if (currentMusicSwapVal > 0.0f)
        {
            currentMusicSwapVal -= Time.deltaTime;
            VolumeSwap();
            return;
        }


        if (tier1MusicOn && tier5MusicOn && currentMusicTrack != 0)
        {
            SwapMusicTrack(0);
            return;
        }

        // We are playing pause music right now:
        if (currentMusicTrack == 0 && tier1MusicOn) return;

        // We are playing boss music right now:
        if (currentMusicTrack == 4 && tier5MusicOn) return;

        // Check if paused:
        if (tier1MusicOn && currentMusicTrack != 0)
        {
            SwapMusicTrack(0);
            return;
        }

        // Check if its boss time:
        if ((tier5MusicOn && currentMusicTrack != 4) || cheer_o_meter == 5 && currentMusicTrack != 4)
        {
            SwapMusicTrack(4);
            return;
        }






        // Check if Cheer-O-Meter allows a music swap:
        if (cheer_o_meter == 2 && currentMusicTrack != 1)
        {
            SwapMusicTrack(1);
        }
        else if (cheer_o_meter == 3 && currentMusicTrack != 2)
        {
            SwapMusicTrack(2);
        }
        else if(cheer_o_meter == 4 && currentMusicTrack != 3)
        {
            SwapMusicTrack(3);
        }
        //Debug.Log(currentMusicTrack);
    }

    private void VolumeSwap()
    {
        float progress = currentMusicSwapVal / musicSwapDuration;

        musicSource[currentMusicTrack].volume = (1-progress)*targetMusicVolume;
        musicSource[prevTrack].volume = progress*targetMusicVolume;
    }

    public void SwapMusicTrack(int swapToTrack)
    {
        currentMusicSwapVal = musicSwapDuration;

        prevTrack = currentMusicTrack;
        currentMusicTrack = swapToTrack;
        foreach (AudioSource a in musicSource)
        {
            a.volume = 0.0f;
        }

        //musicSource[swapToTrack].volume = targetVolume;
    }
    public void PlayMusic(int startingTrack)
    {
        //musicSource[0].Play();
        //musicSource[1].Play();
        //musicSource[2].Play();
        //musicSource[3].Play();
       // musicSource[4].Play();

        foreach (AudioSource a in musicSource)
        {
            a.Play();
            a.volume = 0.0f;
        }

        musicSource[startingTrack].volume = targetMusicVolume;

    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        sfxSource.pitch = Random.Range(1.0f- pitchDiff, 1.0f + pitchDiff);

        if (s == null)
        {
            Debug.Log("No sound found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
            sfxSource.Play();
        }
    }
}
