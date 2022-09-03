using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public bool fishingRodCast = false;
    public bool fishAtHook = false;
    public bool fishingRodReeling = false;

    public bool lookingAtWater = false;

    [SerializeField] GameManager gameManager;
    [SerializeField] FishingController fishingController;
    [SerializeField] public GameObject hook;
    [SerializeField] FishTracker fishTracker;

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
        gameManager.DisplayCaughtFishUI(fishData);

        if (fishTracker.HasNewFishBeenCaught(fishData.Item1)) 
        {
            Debug.Log("new fish caught");
        }
        else
        {
            Debug.Log("Already got this fish");
        }
        //Add fish caught to a dictionary of fish and mark as caught
    }
}
