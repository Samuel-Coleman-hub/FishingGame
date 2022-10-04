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

    [SerializeField] AudioClip castAudio;
    [SerializeField] AudioClip reelAudio;
    [SerializeField] AudioClip bobAudio;
    [SerializeField] AudioClip sinkAudio;

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

    private AudioSource audioSource;


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
        audioSource = GetComponent<AudioSource>();

        fishingRodCast = fishingManager.fishingRodCast;
    }

    private void Fishing(InputAction.CallbackContext context)
    {
        if (fishingManager.fishingRodCast && !stateChanging)
        {
            Reel();
        }
        else if(!fishingManager.fishingRodCast && fishingManager.lookingAtWater && !stateChanging && movement.enabled)
        {
            Cast();
        }
    }

    private void Cast()
    {
        stateChanging = true;
        StartCoroutine(WaitToThrow());
        playerAnimator.SetTrigger("Cast");
        audioSource.clip = castAudio;
        audioSource.Play();
        movement.enabled = false;
        fishingManager.fishingRodCast = true;
    }

    public void Reel()
    {
        stateChanging = true;
        fishingManager.fishingRodReeling = true;
        StartCoroutine(ThrowHook());
        playerAnimator.SetTrigger("Reel");
        audioSource.clip = reelAudio;
        audioSource.Play();
        fishingManager.fishAtHook = false;
    }

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
        else
        {
            audioSource.clip = bobAudio;
            audioSource.Play();
        }

        stateChanging = false;
    }

    private IEnumerator WaitToThrow()
    {
        yield return new WaitForSeconds(hookThrowWaitTime);
        StartCoroutine(ThrowHook());

    }

    public void BobHook()
    {
        hookAnimator.SetTrigger("Bob");
        audioSource.clip = bobAudio;
        audioSource.Play();
    }

    public void SinkHook()
    {
        hookAnimator.SetTrigger("Sink");
        audioSource.clip = sinkAudio;
        audioSource.Play();
    }
}
