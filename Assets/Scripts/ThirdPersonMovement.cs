using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerControls))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float playerRunSpeed = 6.0f;
    //[SerializeField]
    //private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 10f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jogAction;
    //private InputAction jumpAction;

    int isWalkingHash;
    int isJoggingHash;

    private bool isMovementPressed;
    private bool isJogPressed;

    private Vector2 input;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        isWalkingHash = Animator.StringToHash("isWalking");
        isJoggingHash = Animator.StringToHash("isJogging");
        moveAction = playerInput.actions["Move"];
        jogAction = playerInput.actions["Jog"];
        //jumpAction = playerInput.actions["Jump"];

    }

    void Update()
    {
        isMovementPressed = moveAction.inProgress;
        isJogPressed = jogAction.IsPressed();
        
        HandleGravity();
        HandleMovement();
        HandleRotation();
        HandleAnimations();
    }

    private void HandleMovement()
    {
        input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        if (jogAction.IsPressed())
        {
            controller.Move(playerRunSpeed * Time.deltaTime * move);
        }
        else
        {
            controller.Move(playerSpeed * Time.deltaTime * move);
        }
        

        // Changes the height position of the player..
        //if (jumpAction.triggered && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void HandleRotation()
    {
        if (input != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            //Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleAnimations()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJogging = animator.GetBool(isJoggingHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isJogPressed) && !isJogging)
        {
            animator.SetBool(isJoggingHash, true);
        }
        else if ((!isMovementPressed || !isJogPressed) && isJogging)
        {
            animator.SetBool(isJoggingHash, false);
        }
    }

    private void OnDisable()
    {
        //Set animator to idle before disabling movement
        animator.SetBool(isJoggingHash, false);
        animator.SetBool(isWalkingHash, false);
    }

}
