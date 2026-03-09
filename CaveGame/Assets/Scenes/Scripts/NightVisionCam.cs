using UnityEngine;
using System.Collections;
using TMPro;

public class NightVisionCam : MonoBehaviour
{
    public int photosTaken = 0; //Note to future self, make sure a play can't just take 5 photos of the same cave painting
    public float rayDistance = 20f;
    public string cavePaintingTag = "Cave Painting";
    public string capturedTag = "Untagged";
    public float duration = 0.5f;
    public TextMeshProUGUI photoCounter;
    public Light lightFlash;
    public AudioSource flashAudio;
    public Camera scannerCam;
    public LayerMask paintingLayer;
    public bool cameraFlash;
    public float cameraCooldown = 1.5f;
    private bool canTakePhoto = true;
    public Vector3 flashPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        TakePhoto();
    }

    public void TakePhoto()
    {
        if (Input.GetKeyDown(KeyCode.F) && canTakePhoto)
        {
            StartCoroutine(CameraCooldown());
            //Debug.DrawLine();
            Debug.Log("Pressed camera button");
            flashPosition = transform.position; // where the sound came from
            //raycast from the camera's position forward
            Ray ray = scannerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            flashAudio.Play();
            //cameraFlash = true;
            if(!cameraFlash)
            {
                StartCoroutine(Flash());
            }
            if (Physics.Raycast(ray, out hit, rayDistance, paintingLayer))
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 2f);
                //scan for tag of painting
                if (hit.collider.CompareTag(cavePaintingTag))
                {
                   Debug.Log("Took photo of: " + hit.collider.name); 
                    photosTaken++;
                    UpdatePhotoUI();
                    hit.collider.gameObject.tag = capturedTag;
                    Debug.Log("Changed tag to: " + hit.collider.gameObject.tag);
                
                }

                else
                {
                    Debug.Log("Hit nothing");
                }
            }
           
            //if painting, increase counter of photos taken of that painting
            //UI update to show how many photos have been taken of each painting
            // RaycastHit hit;

        }

    }
    void OnDrawGizmosSelected()
    {
        // Set the color of the gizmo to red
        Gizmos.color = Color.yellow;

        // Draw a ray starting from the object's position and going forward
        // transform.forward provides the direction vector relative to the object's rotation
        Gizmos.DrawRay(scannerCam.transform.position, scannerCam.transform.forward * rayDistance);
    }
    IEnumerator Flash()
    {
        lightFlash.intensity = 2f;
        yield return new WaitForSeconds(duration);
        lightFlash.intensity = 0f;
        cameraFlash = false;
    }
    public void UpdatePhotoUI()
    {
        photoCounter.text = ": " + photosTaken.ToString() + "/5";
    }

    IEnumerator CameraCooldown()
    {
        canTakePhoto = false;
        yield return new WaitForSeconds(cameraCooldown);
        canTakePhoto = true;
    }
}
