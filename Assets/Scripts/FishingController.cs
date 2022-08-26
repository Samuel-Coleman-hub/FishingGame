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
    [SerializeField] Transform topOfFishingRod;

    [SerializeField] Transform[] hookPoints;
    [SerializeField] float[] hookThrowSpeeds;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Animator hookContainerAnimator;
    private Animator hookAnimator;
    private Animator fishingRodAnimator;

    private bool fishingRodCast;
    private bool fishAtHook;
    private bool fishingRodReeling;

    private InputAction fish;

    private ThirdPersonMovement movement;
    private LineRenderer lineRenderer;

    private Vector3 waterRelativePos;

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

        waterRelativePos = hookPoints[hookPoints.Length -1].transform.InverseTransformPoint(0,0, 409);
    }

    private void Fishing(InputAction.CallbackContext context)
    {
        if (fishingManager.fishingRodCast)
        {
            Reel();
        }
        else if(!fishingManager.fishingRodCast && fishingManager.lookingAtWater)
        {
            Cast();
        }
    }

    private void Cast()
    {
        hookObj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 100);

        //Detach child hook from armature when casting
        hookContainerObj.transform.parent = this.transform;

        //hookAnimator.SetTrigger("Cast");
        StartCoroutine(ThrowHook());
        playerAnimator.SetTrigger("Cast");
        //fishingRodAnimator.SetTrigger("Cast");
        movement.enabled = false;
        fishingManager.fishingRodCast = true;
    }

    public void Reel()
    {
        fishingManager.fishingRodReeling = true;
        hookAnimator.SetTrigger("Reel");
        playerAnimator.SetTrigger("Reel");
        //fishingRodAnimator.SetTrigger("Reel");
        fishingManager.fishAtHook = false;
        //ScareNearbyFish();
        StartCoroutine(WaitToLift());
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
        int i = 0;
        //hookPoints[hookPoints.Length -1].position = new Vector3(hookPoints[hookPoints.Length -1].position.x, waterRelativePos.y, hookPoints[hookPoints.Length -1].position.z);

        foreach(Transform pos in hookPoints)
        { 
            float time = 0;
            float duration = hookThrowSpeeds[i];
            Vector3 startPosition = hookContainerObj.transform.position;

            while (time < duration)
            {
                hookContainerObj.transform.position = Vector3.Lerp(startPosition, pos.position, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            hookContainerObj.transform.position = pos.position;
            i++;
        }
        
    }

    private IEnumerator WaitToLift()
    {
        yield return new WaitForSeconds(2f);
        hookContainerAnimator.SetTrigger("Rise");
        movement.enabled = true;
        fishingManager.fishingRodCast = false;
        fishingManager.fishingRodReeling = false;
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
