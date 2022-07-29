using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public bool fishingRodCast = false;
    public bool fishAtHook = false;
    public bool fishingRodReeling = false;

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
        fishingController.Reel();
    }
}
