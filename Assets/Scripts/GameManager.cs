using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject fishingObj;
    [SerializeField] GameObject canvasObj;
    [SerializeField] GameObject playerCamera;
    [SerializeField] FishingManager fishingManager;

    [Header("Display Fish Settings")]
    [SerializeField] Transform fishUIDispalyPos;
    [SerializeField] TextMeshProUGUI fishUITitleText;
    [SerializeField] TextMeshProUGUI fishUINameText;
    [SerializeField] AudioClip fishCaughtAudio;

    [Header("Scrapbook Settings")]
    [SerializeField] GameObject scrapbookObj;
    [SerializeField] GameObject scrapbookUI;
    private ScrapBookInterface scrapbookInterface;

    [SerializeField] Volume volume;

    private PlayerInput playerInput;
    private ThirdPersonMovement movement;
    private FishingController fishingController;
    private CinemachineInputProvider cameraInput;

    private InputAction triggerScrapbook;
    private InputAction closeFishUI;

    private bool scrapBookOpen = false;
    private bool fishUIOpen = false;
    private bool blurring = false;
    private bool scrapbookOpening = false;
    private bool displayUIOpening = false;

    private GameObject displayFish;
    private AudioSource audioSource;
    // Start is called before the first frame update

    private void Awake()
    {
        playerInput = canvasObj.GetComponent<PlayerInput>();
        triggerScrapbook = playerInput.actions["Trigger Scrapbook"];
        triggerScrapbook.performed += TriggerScrapbook;

        closeFishUI = playerInput.actions["CloseFishUI"];
        closeFishUI.performed += CloseFishUI;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movement = playerObj.GetComponent<ThirdPersonMovement>();
        fishingController = fishingObj.GetComponent<FishingController>();
        cameraInput = playerCamera.GetComponent<CinemachineInputProvider>();
        scrapbookInterface = scrapbookUI.GetComponent<ScrapBookInterface>();
        audioSource = GetComponent<AudioSource>();
    }

    private void TriggerScrapbook(InputAction.CallbackContext context)
    {
        if (!scrapbookOpening)
        {
            if (!scrapBookOpen && !fishUIOpen)
            {
                scrapbookOpening = true;
                if (!blurring)
                {
                    StartCoroutine(BlurScreen());
                }

                scrapbookObj.SetActive(true);
                scrapbookUI.SetActive(true);
                ToggleMovement();
                scrapBookOpen = true;

            }
            else
            {
                scrapbookObj.SetActive(false);
                scrapbookUI.SetActive(false);
                ToggleMovement();
                scrapBookOpen = false;

                if (!blurring)
                {
                    StartCoroutine(BlurScreen());
                }

            }
        }
        
    }

    private void CloseFishUI(InputAction.CallbackContext context)
    {
        if (fishUIOpen && !displayUIOpening)
        {

            if (!blurring)
            {
                StartCoroutine(BlurScreen());
            }
            StartCoroutine(FadeText(fishUINameText, 0.2f));
            StartCoroutine(FadeText(fishUITitleText, 0.2f));

            GameObject.Destroy(displayFish);
            StartCoroutine(WaitToEnablePlayer());
            fishUIOpen = false;
        }
    }

    public void DisplayCaughtFishUI((FishTracker.Fishes, GameObject) fishData)
    {
        displayUIOpening = true;
        playerObj.GetComponent<FishingController>().enabled = false;

        fishUIOpen = true;
        ToggleMovement();
        if (!blurring)
        {
            StartCoroutine(BlurScreen());
        }
        DisplayFishPrefab(fishData.Item2);

        fishUINameText.text = fishData.Item1.ToString().Replace("_", " ");
        StartCoroutine(FadeText(fishUINameText, 1.5f));
        StartCoroutine(FadeText(fishUITitleText, 1.5f));

        audioSource.clip = fishCaughtAudio;
        audioSource.Play();

        scrapbookInterface.UpdateScrapbook(fishData.Item1);
        Debug.Log(fishingManager.totalFishCaught);
        scrapbookInterface.UpdateTotal(fishingManager.totalFishCaught);

        
    }

    private void DisplayFishPrefab(GameObject fishPrefab)
    {

        //Disable components of fish not needed
        fishPrefab.GetComponent<Rigidbody>().isKinematic = true;
        fishPrefab.GetComponent<NavMeshAgent>().enabled = false;
        fishPrefab.GetComponent<FishStateManager>().enabled = false;
        fishPrefab.GetComponent<Animator>().enabled = true;

        displayFish =
            Instantiate(fishPrefab, fishUIDispalyPos.position, Quaternion.identity) as GameObject;

        //Lower the scale of the fish
        Vector3 displayFishScale = displayFish.transform.localScale;
        displayFish.transform.localScale = new Vector3(displayFishScale.x / 4, displayFishScale.y / 4,
            displayFishScale.z / 4);

        //Rotate the fish
        displayFish.transform.rotation = Quaternion.Euler(0, 90f, 0);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float duration)
    {
        float fadeTo = text.color.a <= 0.2f ? 1f : 0f;
        float fadeFrom = fadeTo == 1f ? 0f : 1f;

        float currentTime = 0f;
        //float duration = 1.5f;

        while (text.color.a != fadeTo)
        {
            float alpha = Mathf.Lerp(fadeFrom, fadeTo, currentTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator BlurScreen()
    {
        blurring = true;
        float time = 0;
        float duration = 1f;
        float startValue;
        float endValue;

        if (volume.profile.TryGet<DepthOfField>(out var depth))
        {
            if(depth.focalLength.value >= 27f)
            {
                startValue = 28f;
                endValue = 19f;
            }
            else
            {
                startValue = 19f;
                endValue = 28f;
            }

            while (time < duration)
            {
                depth.focalLength.value = Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
                blurring = false;
            }
        }
        scrapbookOpening = false;
        displayUIOpening = false;

    }

    private void ToggleMovement()
    {

        movement.enabled = !movement.enabled;
        cameraInput.enabled = !cameraInput.enabled;
    }

    IEnumerator WaitToEnablePlayer()
    {
        yield return new WaitForSeconds(2f);
        ToggleMovement();
        playerObj.GetComponent<FishingController>().enabled = true;
    }
}
