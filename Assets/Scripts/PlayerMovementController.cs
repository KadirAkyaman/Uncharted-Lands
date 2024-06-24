using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private float movementSpeed;
    private float maxMovementSpeed;
    [SerializeField] private float runSpeed;
    private float maxRunSpeed;
    private float gravity;
    public float walkingGravity = -50;
    [SerializeField] private float jumpHeight;

    //GROUND CHECK
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    public bool isGrounded;

    public bool isSwimming;
    public float swimmingGravity = -0.5f;

    //GRASS SOUNDFX
    private Vector3 lastPos = new Vector3(0, 0, 0);
    public bool isMoving;
    public bool isUnderWater;

    void Start()
    {
        isSwimming = false;
        characterController = GetComponent<CharacterController>();
        maxMovementSpeed = movementSpeed;
        maxRunSpeed = runSpeed;
    }

    void Update()
    {
        if (CampfireUIManager.Instance.isUIOpen == false && !SleepSystem.Instance.isSleeping)
        {
            Movement();
        }

    }

    void Movement()
    {


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Zıplama kontrolü
        if (Input.GetButtonDown("Jump") && isGrounded && PlayerState.Instance.canJump)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlayerState.Instance.currentEnergy -= PlayerState.Instance.jumpEnergyLoss;
        }

        // Koşma ve yürüme kontrolü
        if (Input.GetKey(KeyCode.LeftShift) && PlayerState.Instance.canRun && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            characterController.Move(move * runSpeed * Time.deltaTime); // Koşma hızı
        }
        else if (move.magnitude > 0)
        {
            characterController.Move(move * movementSpeed * Time.deltaTime); // Normal yürüme hızı
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


        //IS PLAYER HUNGRY CONTROL => IF HUNGRY => CANT MOVE FAST
        if (PlayerState.Instance.isPlayerHungry)
        {
            movementSpeed = maxMovementSpeed / 2;
            runSpeed = movementSpeed;
        }
        else
        {
            movementSpeed = maxMovementSpeed;
            runSpeed = maxRunSpeed;
        }

        if (isSwimming)
        {
            if (isUnderWater)
            {
                gravity = swimmingGravity;
            }
            else
            {
                velocity.y = 0;
            }
        }
        else
        {
            gravity = walkingGravity;
        }






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
