using UnityEngine;
using UnityEngine.UI;

public class ScannerRadar : MonoBehaviour
{
    public Transform enemy;
    public Image radarLight;      // UI light or material color
    public AudioSource beepAudio;

    //distance between enemy and scanner/player
    public float greenRange = 25f;
    public float yellowRange = 15f;
    public float orangeRange = 7f;

    //closer to the enemy the louder and faster it get
    public float minBeepInterval = 0.2f;
    public float maxBeepInterval = 2f;

    float beepTimer;

    public float currentAlertLevel;
    // 0 = none, 1 = yellow, 2 = orange, 3 = red

    void Update()
    {
        //updates the range between player and enemy
        float distance = Vector3.Distance(transform.position, enemy.position);

        UpdateRadarColor(distance);
        UpdateBeep(distance);
    }

    void UpdateRadarColor(float distance)
    {
        //green then yellow then orange then red
        if (distance > greenRange)
        {
            radarLight.color = Color.green;
            currentAlertLevel = 0;
        }
        else if (distance > yellowRange)
        {
            radarLight.color = Color.yellow;
            currentAlertLevel = 1;
        }
        else if (distance > orangeRange)
        {
            radarLight.color = new Color(1f, 0.5f, 0f); // orange
            currentAlertLevel = 2;
        }
        else
        {
            radarLight.color = Color.red;
            currentAlertLevel = 3;
        }
    }

    void UpdateBeep(float distance)
    {
        float interval = maxBeepInterval; // default
        float volume = 0f; // default

        // Set interval and volume based on alert level
        switch (currentAlertLevel)
        {
            case 1: // yellow
                interval = 1.5f; // slow beep
                volume = 0.2f;  // faint
                break;
            case 2: // orange
                interval = 1.0f; // tiny bit faster
                volume = 0.4f;   // moderate
                break;
            case 3: // red
                interval = 0.7f; // slightly faster than orange
                volume = 0.8f;   // loudest
                break;
        }

        beepTimer -= Time.deltaTime;

        if (beepTimer <= 0f)
        {
            beepAudio.PlayOneShot(beepAudio.clip, volume);
            beepTimer = interval;
        }
    }
}
