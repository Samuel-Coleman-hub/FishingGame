using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject fishingObj;

    public bool lookingAtWater = false;

    private ThirdPersonMovement movement;
    private FishingController fishingController;
    // Start is called before the first frame update

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movement = playerObj.GetComponent<ThirdPersonMovement>();
        fishingController = fishingObj.GetComponent<FishingController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fish") && (lookingAtWater))
        {
            fishingController.Fishing();
        }
        else if((Input.GetButtonUp("Fish")) && !lookingAtWater)
        {
            
        }
    }

    public void ToggleMovement()
    {
        movement.moving = !movement.moving;
    }
}
