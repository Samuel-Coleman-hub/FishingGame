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

    public void FishCaught((string, GameObject) fishData)
    {
        StartCoroutine(gameManager.BlurScreen());
        Debug.Log(fishData.Item1);
    }
}
