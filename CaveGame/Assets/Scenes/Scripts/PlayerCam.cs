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
        //player camera is linked to mouse movement
        //click on game scene to hide mouse
        //press esc on keyboard to get mouse back
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            //delaying the mouse input so that the player doesn't stare at ground when game starts
         timer += Time.deltaTime;
         if (timer < inputDelay) return;

         float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
         float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

         xRotation -= mouseY;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

         transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
         playerBody.Rotate(Vector3.up * mouseX);
         //Debug.Log(Input.GetAxisRaw("Mouse X"));
        }
    }
}
