using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerControls))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float playerRunSpeed = 6.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
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
    private InputAction jumpAction;

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
        jumpAction = playerInput.actions["Jump"];

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
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

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

    //    //[SerializeField] CharacterController controller;
    //    //[SerializeField] Transform cam;

    //    //[SerializeField] float speed = 6f;

    //    //[SerializeField] float turnSmoothTime = 0.1f;
    //    //private float turnSmoothVelocity;

    //    //public bool moving = true;

    //    PlayerControls playerControls;
    //    CharacterController characterController;
    //    Animator animator;

    //    int isWalkingHash;
    //    int isJoggingHash;

    //    Vector2 currentMovementInput;
    //    Vector3 currentMovement;
    //    Vector3 currentJogMovement;
    //    bool isMovementPressed;
    //    bool isJogPressed;
    //    float rotationFactorPerFrame = 1f;
    //    float runMultiplier = 3f;

    //    Transform cameraTransform;

    //    private void Awake()
    //    {
    //        playerControls = new PlayerControls();
    //        characterController = GetComponent<CharacterController>();
    //        animator = GetComponent<Animator>();
    //        cameraTransform = Camera.main.transform;

    //        isWalkingHash = Animator.StringToHash("isWalking");
    //        isJoggingHash = Animator.StringToHash("isJogging");

    //        playerControls.CharacterControls.Move.started += OnMovementInput;
    //        playerControls.CharacterControls.Move.canceled += OnMovementInput;
    //        playerControls.CharacterControls.Move.performed += OnMovementInput;
    //        playerControls.CharacterControls.Jog.started += OnJog;
    //        playerControls.CharacterControls.Jog.canceled += OnJog;
    //    }

    //    private void OnJog(InputAction.CallbackContext context)
    //    {
    //        isJogPressed = context.ReadValueAsButton();
    //    }

    //    private void OnMovementInput(InputAction.CallbackContext context)
    //    {
    //        currentMovementInput = context.ReadValue<Vector2>();
    //        currentMovement.x = currentMovementInput.x;
    //        currentMovement.z = currentMovementInput.y;
    //        currentJogMovement.x = currentMovementInput.x * runMultiplier;
    //        currentJogMovement.z = currentMovementInput.y * runMultiplier;
    //        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    //    }

    //    private void HandleRotation()
    //    {
    //        Vector3 positionToLookAt;
    //        positionToLookAt.x = currentMovement.x;
    //        positionToLookAt.y = 0.0f;
    //        positionToLookAt.z = currentMovement.z;

    //        Quaternion currentRotation = transform.rotation;

    //        if (isMovementPressed)
    //        {
    //            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
    //            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
    //        }

    //    }

    //    private void HandleAnimation()
    //    {
    //        bool isWalking = animator.GetBool(isWalkingHash);
    //        bool isJogging = animator.GetBool(isJoggingHash);

    //        if(isMovementPressed && !isWalking)
    //        {
    //            animator.SetBool(isWalkingHash, true);

    //        }
    //        else if(!isMovementPressed && isWalking)
    //        {
    //            animator.SetBool(isWalkingHash, false);
    //        }

    //        if((isMovementPressed && isJogPressed) && !isJogging)
    //        {
    //            animator.SetBool(isJoggingHash, true);
    //        }
    //        else if((!isMovementPressed || !isJogPressed) && isJogging)
    //        {
    //            animator.SetBool(isJoggingHash, false);
    //        }
    //    }

    //    private void HandleGravity()
    //    {
    //        if (characterController.isGrounded)
    //        {
    //            float groundedGravity = -.05f;
    //            currentMovement.y = groundedGravity;
    //            currentJogMovement.y = groundedGravity;
    //        }
    //        else
    //        {
    //            float gravity = -9.8f;
    //            currentMovement.y += gravity;
    //            currentJogMovement.y += gravity;
    //        }
    //    }

    //    private void Update()
    //    {
    //        //HandleRotation();
    //        HandleAnimation();

    //        if (isJogPressed)
    //        {
    //            ////Ive added this
    //            //currentJogMovement = currentJogMovement.x * cameraTransform.right.normalized + currentJogMovement.z *
    //            //    cameraTransform.forward.normalized;
    //            ////currentJogMovement.y = 0f;
    //            characterController.Move(currentJogMovement * Time.deltaTime);
    //        }
    //        else
    //        {
    //            currentMovement = currentMovement.x * cameraTransform.right.normalized + currentMovement.z *
    //                cameraTransform.forward.normalized;
    //            //currentMovement.y = 0f;
    //            characterController.Move(currentMovement * Time.deltaTime);
    //        }

    //    }

    //    private void OnEnable()
    //    {
    //        playerControls.CharacterControls.Enable();
    //    }

    //    private void OnDisable()
    //    {
    //        playerControls.CharacterControls.Disable();
    //    }

    //    //private void Update()
    //    //{
    //    //    if (moving)
    //    //    {
    //    //        Movement();
    //    //    }
    //    //}

    //    //private void Movement()
    //    //{
    //    //    float horizontal = Input.GetAxisRaw("Horizontal");
    //    //    float vertical = Input.GetAxisRaw("Vertical");
    //    //    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    //    //    if (direction.magnitude >= 0.1f)
    //    //    {
    //    //        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
    //    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //    //        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //    //        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    //    //    }
    //    //}
}
