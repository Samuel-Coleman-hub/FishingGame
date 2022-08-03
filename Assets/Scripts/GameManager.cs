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

    public bool lookingAtWater = false;

    private PlayerInput playerInput;
    private ThirdPersonMovement movement;
    private FishingController fishingController;

    private InputAction openScrapbook;
    // Start is called before the first frame update

    private void Awake()
    {
        playerInput = canvasObj.GetComponent<PlayerInput>();
        openScrapbook = playerInput.actions["Open Scrapbook"];
        openScrapbook.started += OpenScrapbook;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movement = playerObj.GetComponent<ThirdPersonMovement>();
        fishingController = fishingObj.GetComponent<FishingController>();
    }

    private void OpenScrapbook(InputAction.CallbackContext context)
    {
        Debug.Log("Open menu");
        scrapbookObj.SetActive(true);
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
