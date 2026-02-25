using UnityEngine;

public class NightVisionCam : MonoBehaviour
{
    public int photosTaken = 0;

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

            //raycast from the camera's position forward
            //scan for tag of painting
            //if painting, increase counter of photos taken of that painting
            //UI update to show how many photos have been taken of each painting
            // RaycastHit hit;

        }

    }

    public void UpdatePhotoUI()
    {

    }
}
