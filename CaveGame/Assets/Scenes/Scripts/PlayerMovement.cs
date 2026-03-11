using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float sprintMultiplier = 2f;

    public float jumpHeight = 1.2f;
    //stronger gravity for heavier jump
    public float gravity = -20f;   
    //fall faster
    public float fallMultiplier = 2.5f;   

    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;

    private AudioSource walking;
    private AudioSource running;

    public GameObject BarrierWall;

    private void Awake()
    {
        AudioSource[] footsteps = GetComponents<AudioSource>();
        walking = footsteps[0];
        running = footsteps[1];

        walking.loop=true;
        running.loop=true;
    }

    void Update()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isSprinting = false; // reset every frame
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keeps player grounded
            
        }

        //movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        float forwardSpeed = speed;
        bool movingForwardOrBack = Mathf.Abs(z) > 0.1f;

        //sprint with left/right shift and only for moving forward and backward

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && movingForwardOrBack)
        {
            forwardSpeed *= sprintMultiplier;
            isSprinting = true;
        }

        // Apply different speeds
        Vector3 move =
            transform.right * x * speed +           // sideways normal speed
            transform.forward * z * forwardSpeed;   // forward/back sprint speed

        controller.Move(move * Time.deltaTime);

        //Footstep audio
        if (isGrounded && isMoving)
        {
            if(isSprinting)
            {
                if(!running.isPlaying)
                {
                    walking.Stop();
                    running.Play();
                }
            }
            else
            {
                if (!walking.isPlaying)
                {
                    running.Stop();
                    walking.Play();
                }

            }
        }
        else
        {
            walking.Pause();
            running.Pause();
        }

        // jump using space key
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;

        // Extra fall speed when going down
        if (velocity.y < 0)
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == BarrierWall)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            //Load EndScreen
            SceneManager.LoadScene("EndScreen");
        }
    }
}
