using UnityEngine;

public class FishReelState : FishBaseState
{
    private float stopWatch;
    private bool stopWatchStart;
    public override void EnterState(FishStateManager fish)
    {
        stopWatch = 0;
        ReelInFish(fish);
    }

    public override void UpdateState(FishStateManager fish)
    {
        if (stopWatchStart)
        {
            stopWatch += Time.deltaTime;
            if(stopWatch >= 3f)
            {
                GameObject.Destroy(fish.gameObject);
            }
        }
    }

    private void ReelInFish(FishStateManager fish)
    {
        fish.animator.enabled = false;
        fish.hookHinge.connectedBody = fish.rb;
        fish.fishingManager.fishAtHook = false;
        stopWatchStart = true;
    }
}
