using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject fishingObj;
    [SerializeField] GameObject scrapbookObj;
    [SerializeField] GameObject canvasObj;
    [SerializeField] GameObject playerCamera;

    private PlayerInput playerInput;
    private ThirdPersonMovement movement;
    private FishingController fishingController;
    private CinemachineInputProvider cameraInput;

    private InputAction triggerScrapbook;

    private bool scrapBookOpen = false;
    // Start is called before the first frame update

    private void Awake()
    {
        playerInput = canvasObj.GetComponent<PlayerInput>();
        triggerScrapbook = playerInput.actions["Trigger Scrapbook"];
        triggerScrapbook.performed += TriggerScrapbook;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movement = playerObj.GetComponent<ThirdPersonMovement>();
        fishingController = fishingObj.GetComponent<FishingController>();
        cameraInput = playerCamera.GetComponent<CinemachineInputProvider>();
    }

    private void TriggerScrapbook(InputAction.CallbackContext context)
    {
        if (!scrapBookOpen)
        {
            scrapbookObj.SetActive(true);
            movement.enabled = false;
            cameraInput.enabled = false;
            scrapBookOpen = true;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            scrapbookObj.SetActive(false);
            movement.enabled = true;
            cameraInput.enabled = true;
            scrapBookOpen = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonUp("Fish") && (lookingAtWater))
        //{
        //    fishingController.Fishing();
        //}
        //else if((Input.GetButtonUp("Fish")) && !lookingAtWater)
        //{
            
        //}
    }

    public void ToggleMovement()
    {
        movement.enabled = false;
    }
}
