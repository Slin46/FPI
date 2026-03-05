using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Sliders : MonoBehaviour
{
    public TMP_Text musicText;
    public TMP_Text sfxText;
    public TMP_Text sensitivityText;

    public static float mouseSensitivity = 320f;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public TMPro.TMP_Text volumeText;

    public void UpdateVolume(float value)
    {
        AudioListener.volume = value;

        int percent = Mathf.RoundToInt(value * 100);
        volumeText.text = "" + percent;
    }
    public void SetMusic(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        musicText.text = "" + percent;

        // replace later with audio mixer
    }

    public void SetSFX(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        sfxText.text = "" + percent;

        // replace later with audio mixer
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;

        sensitivityText.text = "" + Mathf.RoundToInt(value);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
