using UnityEngine;

public class FishEscapeState : FishBaseState
{
    private float stopWatch;
    private bool stopWatchStart;
    public override void EnterState(FishStateManager fish)
    {
        stopWatch = 0;
        FishEscape(fish);
    }

    public override void UpdateState(FishStateManager fish)
    {
        if (stopWatchStart)
        {
            stopWatch += Time.deltaTime;
            if(stopWatch >= fish.waitTimeToDie)
            {
                GameObject.Destroy(fish.gameObject);
            }
        }
    }

    public void FishEscape(FishStateManager fish)
    {
        fish.fishingManager.RespawnEscapedFish(fish.respawnLocation, fish.typeOfFish);
        fish.agent.enabled = true;
        fish.swimPointRange = 15f;
        fish.FindSwimPoint();
        fish.agent.speed *= 4;
        fish.agent.SetDestination(fish.swimPoint);
        fish.fishingManager.TriggerReel();
        stopWatchStart = true;
    }

}
