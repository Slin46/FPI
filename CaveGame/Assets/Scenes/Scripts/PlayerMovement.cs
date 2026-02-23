using UnityEngine;

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

    void Update()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keeps player grounded
        }

        //movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float forwardSpeed = speed;
        bool movingForwardOrBack = Mathf.Abs(z) > 0.1f;

        //sprint ith left/right shift and only for moving forward and backward
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && movingForwardOrBack)
            forwardSpeed *= sprintMultiplier;

        // Apply different speeds
        Vector3 move =
            transform.right * x * speed +           // sideways normal speed
            transform.forward * z * forwardSpeed;   // forward/back sprint speed

        controller.Move(move * Time.deltaTime);

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
    
}
