using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private TextMeshProUGUI volumeText;

    [SerializeField]
    private Slider FOVSlider;
    [SerializeField]
    private TextMeshProUGUI FOVSText;

    [SerializeField]
    private Slider sensitivitySlider;
    [SerializeField]
    private TextMeshProUGUI sensitivityText;

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private GameMaster GameMaster;
    public void UpdateVolume()
    {
        volumeText.text = Mathf.Round(((volumeSlider.value)/0.2f)*100).ToString() + "%";
        music.volume = volumeSlider.value;
    }

    public void UpdateFOV()
    {
        FOVSText.text = Mathf.Round(FOVSlider.value).ToString();
    }

    public void UpdateSensitivity()
    {
        sensitivityText.text = Mathf.Round(sensitivitySlider.value).ToString();
    }

    public void SetGameMaster()
    {
        GameMaster.volume = volumeSlider.value;
        GameMaster.FOV = Mathf.Round(FOVSlider.value);
        GameMaster.sensitivity = Mathf.Round(sensitivitySlider.value);
    }

}
