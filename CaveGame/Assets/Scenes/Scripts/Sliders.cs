using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    [Header("Text Displays")]
    public TMP_Text masterText;
    public TMP_Text musicText;
    public TMP_Text sfxText;
    public TMP_Text sensitivityText;

    [Header("Slider References")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;

    [Header("Mouse Settings")]
    public static float mouseSensitivity = 320f;

    //save the settings when loading into game scene
    public static class PlayerSettings
    {
        public static float masterVolume = 0.5f;
        public static float musicVolume = 0.5f;
        public static float sfxVolume = 0.5f;
        public static float mouseSensitivity = 320f;
    }

    [Header("Audio Mixer")]
    public AudioMixer mixer; // assign in Inspector if you want separate control later

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
        PlayerSettings.masterVolume = value;   // save to static class

        if (masterText != null)
            masterText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void SetMusicVolume(float value)
    {
        PlayerSettings.musicVolume = value;

        if (musicText != null)
            musicText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void SetSFXVolume(float value)
    {
        PlayerSettings.sfxVolume = value;

        if (sfxText != null)
            sfxText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void SetSensitivity(float value)
    {
        PlayerSettings.mouseSensitivity = value;

        if (sensitivityText != null)
            sensitivityText.text = Mathf.RoundToInt(value).ToString();
    }

    // set defaults at start
    void Start()
    {
        // Apply saved settings to sliders
        if (masterSlider != null)
            masterSlider.value = PlayerSettings.masterVolume;
        if (musicSlider != null)
            musicSlider.value = PlayerSettings.musicVolume;
        if (sfxSlider != null)
            sfxSlider.value = PlayerSettings.sfxVolume;
        if (sensitivitySlider != null)
            sensitivitySlider.value = PlayerSettings.mouseSensitivity;

        // Apply to text & audio
        SetMasterVolume(PlayerSettings.masterVolume);
        SetMusicVolume(PlayerSettings.musicVolume);
        SetSFXVolume(PlayerSettings.sfxVolume);
        SetSensitivity(PlayerSettings.mouseSensitivity);

    }
}
