using UnityEngine;

public class FishHookedState : FishBaseState
{
    private float stopWatch;

    public override void EnterState(FishStateManager fish)
    {
        stopWatch = 0;
        fish.agent.enabled = false;
        fish.fishingManager.SinkHook();
    }

    public override void UpdateState(FishStateManager fish)
    {
        stopWatch += Time.deltaTime;

        if (fish.fishingManager.fishingRodReeling)
        {
            fish.SwitchState(fish.reelState);
        }
        else if (stopWatch >= fish.waitTimeToCatch)
        {
            Debug.Log("Fish Got away");
            fish.SwitchState(fish.escapeState);
        }
    }
}
