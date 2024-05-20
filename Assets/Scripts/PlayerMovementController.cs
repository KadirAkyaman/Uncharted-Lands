using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private float movementSpeed;
    private float gravity;
    public float walkingGravity = -50;
    [SerializeField] private float jumpHeight;

    //GROUND CHECK
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    public bool isSwimming;
    public float swimmingGravity = -0.5f;

    //GRASS SOUNDFX
    private Vector3 lastPos = new Vector3(0,0,0);
    public bool isMoving;
    public bool isUnderWater;

    void Start()
    {
        isSwimming = false;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (CampfireUIManager.Instance.isUIOpen == false)
        {
            Movement();
        }
        
    }

    void Movement()
    {
        if (isSwimming)
        {
            if (isUnderWater)
            {
                
            }
            gravity = swimmingGravity;
        }
        else
        {
            gravity = walkingGravity;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * movementSpeed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);


        //WALK SFX
        if (lastPos != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;

            SoundManager.Instance.grassWalkSound.Stop();
        }

        lastPos = gameObject.transform.position;
    }
}
