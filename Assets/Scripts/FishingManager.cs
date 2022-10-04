using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public int totalFishCaught = 0;
    public bool fishingRodCast = false;
    public bool fishAtHook = false;
    public bool fishingRodReeling = false;

    public bool lookingAtWater = false;

    [SerializeField] GameManager gameManager;
    [SerializeField] FishingController fishingController;
    [SerializeField] public GameObject hook;
    [SerializeField] FishTracker fishTracker;
    [SerializeField] FishSpawner fishSpawner;
    [SerializeField] AudioClip fishSplashWaterAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameManager.GetComponent<AudioSource>();
    }

    public void BobHook()
    {
        fishingController.BobHook();
    }

    public void SinkHook()
    {
        fishingController.SinkHook();
    }
    
    public void TriggerReel()
    {
        if (fishingRodCast)
        {
            fishingController.Reel();
        }
    }

    public void FishCaught((FishTracker.Fishes, GameObject) fishData)
    {
        audioSource.clip = fishSplashWaterAudio;
        audioSource.Play();
        
        if (fishTracker.HasNewFishBeenCaught(fishData.Item1)) 
        {
            totalFishCaught++;
        }

        gameManager.DisplayCaughtFishUI(fishData);
        //Add fish caught to a dictionary of fish and mark as caught
    }

    public void RespawnEscapedFish(Vector3 spawnPosition, FishTracker.Fishes fishType)
    {
        StartCoroutine(fishSpawner.WaitToRespawn(spawnPosition, fishType));
    }
}
