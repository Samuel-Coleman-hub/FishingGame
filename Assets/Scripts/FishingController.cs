using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingController : MonoBehaviour
{
    [SerializeField] FishSpawner fishSpawner;
    [SerializeField] GameObject hookContainerObj;
    [SerializeField] GameObject hookObj;

    public bool hookOccupied = false;
    public bool casting = false;

    private PlayerInput playerInput;
    private Animator hookContainerAnimator;
    private Animator hookAnimator;

    private InputAction fish;

    private ThirdPersonMovement movement;

    //private List<FishController> fishes = new List<FishController>();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fish = playerInput.actions["Fish"];
        fish.started += Fishing;
    }

    private void Start()
    {
        hookContainerAnimator = hookContainerObj.GetComponent<Animator>();
        hookAnimator = hookObj.GetComponent<Animator>();
        movement = GetComponent<ThirdPersonMovement>();
    }

    private void Fishing(InputAction.CallbackContext context)
    {
        if (casting)
        {
            Reel();
        }
        else
        {
            Cast();
        }
    }

    private void Cast()
    {
        hookAnimator.SetTrigger("Cast");
        movement.enabled = false;
        casting = true;
    }

    public void Reel()
    {
        hookAnimator.SetTrigger("Reel");
        hookOccupied = false;
        //ScareNearbyFish();
        //StartCoroutine(WaitToLift());
        
    }

    //private void ScareNearbyFish()
    //{
    //    foreach(FishController fish in fishes)
    //    {
    //        fish.FishEscape();
    //    }
    //}

    //private IEnumerator WaitToLift()
    //{
    //    yield return new WaitForSeconds(2f);
    //    hookBobAnimator.SetTrigger("Rise");
    //    gameManager.ToggleMovement();
    //    casting = false;
    //}

    //Called at the end of th Reel Animation
    private void ResetAfterReel()
    {
        hookContainerAnimator.SetTrigger("Rise");
        movement.enabled = true;
        casting = false;
    }

    public void BobHook()
    {
        hookContainerAnimator.SetTrigger("Bob");
    }

    public void SinkHook()
    {
        hookContainerAnimator.SetTrigger("Sink");
    }
}
