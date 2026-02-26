using UnityEngine;
using System.Collections;

public class NightVisionCam : MonoBehaviour
{
    public int photosTaken = 0; //Note to future self, make sure a play can't just take 5 photos of the same cave painting
    public float rayDistance = 5f;
    public string cavePaintingTag = "Cave Painting";
    public float duration = 0.5f;
    public Light lightFlash;
    public AudioSource flashAudio;
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pressed camera button");
            //raycast from the camera's position forward
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            flashAudio.Play();
            StartCoroutine(Flash());
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                //scan for tag of painting
                if (hit.collider.CompareTag(cavePaintingTag))
                {
                   Debug.Log("Took photo of: " + hit.collider.name); 
                    photosTaken++;
                    UpdatePhotoUI();
                
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
    
    IEnumerator Flash()
    {
        lightFlash.intensity = 2f;
        yield return new WaitForSeconds(duration);
        lightFlash.intensity = 0f;
    }
    public void UpdatePhotoUI()
    {

    }
}
