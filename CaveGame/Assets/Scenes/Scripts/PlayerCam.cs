using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform playerBody;

    float xRotation = 0f;
    float inputDelay = 0.1f;
    float timer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        xRotation = 0f;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        // Only move camera if mouse is locked
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            timer += Time.deltaTime;
            if (timer < inputDelay) return;

            // Use the value from PlayerSettings
            float currentSensitivity = Sliders.PlayerSettings.mouseSensitivity;

            float mouseX = Input.GetAxisRaw("Mouse X") * currentSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * currentSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
