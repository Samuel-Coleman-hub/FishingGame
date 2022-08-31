using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingController : MonoBehaviour
{
    [SerializeField] FishSpawner fishSpawner;
    [SerializeField] FishingManager fishingManager;
    [SerializeField] GameObject hookContainerObj;
    [SerializeField] GameObject hookObj;
    [SerializeField] GameObject fishingRodObj;
    [SerializeField] Transform hookHolder;

    [SerializeField] Transform[] hookPoints;
    [SerializeField] float[] hookThrowSpeeds;
    [SerializeField] float hookThrowWaitTime;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Animator hookContainerAnimator;
    private Animator hookAnimator;
    private Animator fishingRodAnimator;

    private bool fishingRodCast;
    private bool fishAtHook;
    private bool fishingRodReeling;
    private bool stateChanging;

    private InputAction fish;

    private ThirdPersonMovement movement;
    private LineRenderer lineRenderer;

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
        playerAnimator = GetComponent<Animator>();

        fishingRodCast = fishingManager.fishingRodCast;
    }

    private void Fishing(InputAction.CallbackContext context)
    {
        if (fishingManager.fishingRodCast && !stateChanging)
        {
            Reel();
        }
        else if(!fishingManager.fishingRodCast && fishingManager.lookingAtWater && !stateChanging)
        {
            Cast();
        }
    }

    private void Cast()
    {
        stateChanging = true;
        //Detach child hook from armature when casting
        //hookAnimator.SetTrigger("Cast");
        //StartCoroutine(ThrowHook());
        StartCoroutine(WaitToThrow());
        playerAnimator.SetTrigger("Cast");
        //fishingRodAnimator.SetTrigger("Cast");
        movement.enabled = false;
        fishingManager.fishingRodCast = true;
    }

    public void Reel()
    {
        stateChanging = true;
        fishingManager.fishingRodReeling = true;
        StartCoroutine(ThrowHook());
        playerAnimator.SetTrigger("Reel");
        fishingManager.fishAtHook = false;
        //fishingRodAnimator.SetTrigger("Reel");
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

    private IEnumerator ThrowHook()
    {
        hookContainerObj.transform.parent = this.transform;

        if (fishingManager.fishingRodReeling)
        {
            System.Array.Reverse(hookPoints);
        }
        
        for (int i = 0; i < hookPoints.Length; i++)
        {
            float time = 0;
            float duration = hookThrowSpeeds[i];
            Vector3 startPosition = hookContainerObj.transform.position;

            while (time < duration)
            {
                hookContainerObj.transform.position = Vector3.Lerp(startPosition, hookPoints[i].position, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            hookContainerObj.transform.position = hookPoints[i].position;
            hookContainerObj.transform.rotation = Quaternion.identity;
        }

        if (fishingManager.fishingRodReeling)
        {
            System.Array.Reverse(hookPoints);
            hookAnimator.SetTrigger("Rise");
            hookContainerObj.transform.parent = hookHolder.transform;
            hookContainerObj.transform.position = hookHolder.position;
            movement.enabled = true;
            fishingManager.fishingRodCast = false;
            fishingManager.fishingRodReeling = false;
        }

        stateChanging = false;
    }

    private IEnumerator WaitToThrow()
    {
        yield return new WaitForSeconds(hookThrowWaitTime);
        StartCoroutine(ThrowHook());

    }

    //private IEnumerator WaitToLift()
    //{
    //    yield return new WaitForSeconds(2f);
    //    hookContainerAnimator.SetTrigger("Rise");
    //    movement.enabled = true;
    //    fishingManager.fishingRodCast = false;
    //    fishingManager.fishingRodReeling = false;
    //}

    public void BobHook()
    {
        Debug.Log("Bob");
        hookAnimator.SetTrigger("Bob");
    }

    public void SinkHook()
    {
        Debug.Log("Sink");
        hookAnimator.SetTrigger("Sink");
    }
}
